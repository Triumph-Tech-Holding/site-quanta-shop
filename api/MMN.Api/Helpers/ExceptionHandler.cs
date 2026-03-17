using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System;
using MMN.Dominio.ViewModel;
using MMN.Dominio.Excecao;
using System.Resources;

namespace SimplesmenteSou.Configuration
{
    public static class ExceptionHandler
    {
        public static void GlobalExceptionHandler(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async context =>
            {
                IEnumerable<Exception> errors;
                ExcecaoViewModel response;
                string mensagem;

                var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                var mensagensErro = new List<MensagemErroViewModel>();

                var error = exceptionHandlerPathFeature?.Error;

                if (error is AggregateException aggregateException)
                {
                    errors = aggregateException.Flatten().InnerExceptions;
                }
                else
                {
                    errors = new Exception[] { error };
                }

                var mensagemErroInternoResource = new ResourceManager(
                    "MMN.Dominio.Excecao.MensagemErroInterno",
                    typeof(PadraoException).Assembly
                );

                var padraoExceptions = errors.Where(w => w is PadraoException);
                if (padraoExceptions.Any())
                {
                    foreach (var exception in padraoExceptions)
                    {
                        var padraoException = exception as PadraoException;
                        var viewModel = padraoException.CriarViewModel();

                        if (viewModel.Mensagem == null)
                        {
                            viewModel.Mensagem = mensagemErroInternoResource
                                .GetString($"{context.Request.Method} {context.Request.Path}");
                            if (viewModel.Mensagem == null)
                            {
                                viewModel.Mensagem = mensagemErroInternoResource
                                    .GetString("erro_interno");
                            }
                        }

                        mensagensErro.Add(viewModel);
                    }
                }

                var internalErrors = errors.Where(w => !(w is PadraoException));
                if (internalErrors.Any())
                {
                    var request = $"{context.Request.Method} {context.Request.Path.Value.Replace("/api", "")}";

                    mensagem = mensagemErroInternoResource.GetString(request);
                    if (mensagem == null)
                    {
                        request = string.Join("/", request.Split("/").Take(3));
                        mensagem = mensagemErroInternoResource.GetString(request);
                        if (mensagem == null)
                        {
                            mensagem = mensagemErroInternoResource.GetString("erro_interno");
                        }
                    }

                    mensagensErro.Add(
                        new MensagemErroViewModel
                        {
                            ErrorCode = "erro_interno",
                            Mensagem = mensagem
                        });

#if !DEBUG
                    if (context.Request.Cookies["debug"] == "oh_vida_oh_ceus")
#endif
                    {
                        if (error is AggregateException)
                        {
                            mensagensErro.Add(
                                new MensagemErroViewModel
                                {
                                    ErrorCode = "erro_interno",
                                    Mensagem = $"{error.Message}<br/>\n--------<br/>\n{error.StackTrace}"
                                });
                        }

                        foreach (var internalError in internalErrors)
                        {
                            mensagensErro.Add(
                                new MensagemErroViewModel
                                {
                                    ErrorCode = "erro_interno",
                                    Mensagem = $"{internalError.Message}<br/>\n--------<br/>\n{internalError.StackTrace}"
                                });

                            if (internalError.InnerException != null)
                            {
                                mensagensErro.Add(
                                    new MensagemErroViewModel
                                    {
                                        ErrorCode = "erro_interno",
                                        Mensagem = $"{internalError.InnerException.Message}<br/>\n--------<br/>\n{internalError.InnerException.StackTrace}"
                                    });
                            }
                        }
                    }
                }

                if (errors.Any(w => w is UnauthorizedException))
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else if (errors.Any(w => w is NotFoundException))
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else if (padraoExceptions.Any())
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                context.Response.ContentType = "application/json";
                response = new ExcecaoViewModel
                {
                    Erros = mensagensErro
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        }
    }
}
