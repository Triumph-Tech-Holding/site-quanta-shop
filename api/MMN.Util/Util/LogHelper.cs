using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace MMN.Util.Util
{
    /// <summary>
    /// Classe de ajuda de geração de Logs no sistema
    /// </summary>
    /// 

    public class LogHelper
    {

        public static bool? Log;
        public static bool? LogErro;

        public static void Trace(Exception ex)
        {

            if (LogErro ?? false)
            {
                var st = new StackTrace(ex);
                var metodo = ("Método: " + st.GetFrame(0).GetMethod().Name);
                GravarLogErro(ex, metodo, ex.StackTrace);
            }
            else
            {
                try
                {
                    // Registro de erros SQL
                    if (ex is SqlException exception)
                    {
                        var innerException = exception.InnerException;
                        if (innerException != null)
                            LogException("Erro de transação SQL. Código: " +
                                         exception.Number + "\nExceção: " +
                                         innerException);
                        TraceSql(ex);
                        return;
                    }
                    var sDiretorioLog = AppDomain.CurrentDomain.BaseDirectory;

                    // Tratamento para o erro "Thread was being aborted" / "Thread estava sendo anulado"
                    if (ex is ThreadAbortException) sDiretorioLog += "LogErroThreadAbort.txt";
                    else sDiretorioLog += "LogErroTrace.txt";

                    var stackTraceMethod = ex.StackTrace.Split('\n');
                    var metodo = stackTraceMethod.Last();

                    var mensagem = new StringBuilder();
                    mensagem.AppendLine(DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss"));
                    mensagem.AppendLine("Erro ao executar: " + metodo);
                    mensagem.AppendLine("Exceção: " + ex.Message.Replace("\n", " ").Replace("<br>", " "));
                    mensagem.AppendLine("StackTrace: " + ex.StackTrace);
                    try
                    {
                        StackTrace st = new StackTrace(ex);
                        mensagem.AppendLine("Método: " + st.GetFrame(0).GetMethod().Name);
                    }
                    catch
                    {
                        // ignored
                    }

                    var inner = ex.InnerException;
                    while (inner != null)
                    {
                        mensagem.AppendLine("Exceção Interna: " + inner.Message.Replace("\n", " ").Replace("<br>", " "));
                        inner = inner.InnerException;
                    }
                    try
                    {
                        var memberInfo = ex.GetType().BaseType;
                        if (memberInfo != null)
                            mensagem.AppendLine("Tipo de exceção: " + memberInfo.FullName);
                    }
                    catch
                    {
                        // ignored
                    }

                    var sw = File.AppendText(sDiretorioLog);
                    sw.WriteLine(mensagem.ToString());
                    sw.Close();
                }
                catch (Exception ex2)
                {
                    Trace(ex2);
                }
            }
        }

        public static void LogException(string expression, Exception ex = null, string nomeArquivo = null)
        {
            if (Log ?? false)
            {
                GravaLog(expression);
            }
            else
            {
                try
                {
                    var caminhoLogErro = AppDomain.CurrentDomain.BaseDirectory + (nomeArquivo ?? "LogErro.txt");
                    if (!File.Exists(caminhoLogErro)) { File.Create(caminhoLogErro).Close(); }
                    var sw = File.AppendText(caminhoLogErro);

                    sw.WriteLine($"[{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}] {expression}");

                    if (ex != null)
                    {
                        sw.WriteLine($"Exceção: {ex.Message}");
                        sw.WriteLine($"Source: {ex.Source}");
                        var aux = ex.InnerException;
                        while (aux != null)
                        {
                            sw.WriteLine($"Exceção interna: {aux.Message}");
                            aux = aux.InnerException;
                        }

                        try
                        {
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                            if (ex != null)
                            {
                                sw.WriteLine("StackTrace: " + ex.StackTrace);
                                var st = new StackTrace(ex);
                                sw.WriteLine("Método: " + st.GetFrame(0).GetMethod().Name);
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    sw.Close();
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void TraceSql(Exception e)
        {
            try
            {
                if (!(e is SqlException sqlEx)) return;
                var sDiretorioLog = AppDomain.CurrentDomain.BaseDirectory;
                sDiretorioLog += "LogErroSql.txt";
                var stackTraceMethod = sqlEx.StackTrace.Split('\n');
                var metodo = stackTraceMethod.Last();
                var mensagem = new StringBuilder();
                mensagem.AppendLine(DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss"));
                mensagem.AppendLine("Erro ao executar: " + metodo);
                mensagem.AppendLine("Exceção: " + sqlEx.Message.Replace("\n", " ").Replace("<br>", " "));
                mensagem.AppendLine("Código do erro: " + sqlEx.Number);
                var sw = File.AppendText(sDiretorioLog);
                sw.WriteLine(mensagem.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                Trace(ex);
            }
        }

        public static void GravaLog(string msg)
        {
            switch (Log)
            {
                case true:
                    var location = Assembly.GetExecutingAssembly().Location;
                    var path = location.Remove(location.LastIndexOf(Assembly.GetExecutingAssembly().GetName().Name, StringComparison.Ordinal));

                    if (!Directory.Exists(path + "Log"))
                    {
                        Directory.CreateDirectory(path + "Log");
                    }
                    if (!File.Exists(path + @"log/GravaLog.txt"))
                    {
                        var vWriter = new StreamWriter(path + @"log\GravaLog.txt", true);
                        vWriter.WriteLine(msg ?? "");
                        vWriter.Flush();
                        vWriter.Close();
                    }
                    else
                    {
                        var vWriter = new StreamWriter(path + @"log\GravaLog.txt", true);
                        vWriter.WriteLine(msg ?? "");
                        vWriter.Flush();
                        vWriter.Close();
                    }

                    //var vWriter = new StreamWriter(path + @"log\log.txt", true);
                    //vWriter.WriteLine(msg ?? "");
                    //vWriter.Flush();
                    //vWriter.Close();
                    break;
            }
        }

        public static void GravarLogErro(Exception ex, string classe, string metodo)
        {
            if (LogErro != true) return;

            var location = Assembly.GetExecutingAssembly().Location;
            var path = location.Remove(location.LastIndexOf(Assembly.GetExecutingAssembly().GetName().Name, StringComparison.Ordinal));

            if (!Directory.Exists(path + "Log"))
            {
                Directory.CreateDirectory(path + "Log");
            }
            StreamWriter vWriter = new StreamWriter(path + @"log\GravaLogErro.txt", true);
            if (!File.Exists(path + @"log\GravaLogErro.txt"))
            {
                vWriter.WriteLine("Notification Service MMN");
                vWriter.WriteLine("Data: " + DateTime.UtcNow.ToString("G"));
                vWriter.WriteLine("Classe - " + classe);
                vWriter.WriteLine("Metodo - " + metodo);
                vWriter.WriteLine("Erro - " + ex.Message);
            }
            else
            {
                vWriter.WriteLine("Notification Service MMN");
                vWriter.WriteLine("Data: " + DateTime.UtcNow.ToString("G"));
                vWriter.WriteLine("Classe - " + classe);
                vWriter.WriteLine("Metodo - " + metodo);
                vWriter.WriteLine("Erro - " + ex.Message);
            }

            var aux = ex.InnerException;
            while (aux != null)
            {
                vWriter.WriteLine("Inner Exception: - " + aux.Message);
                aux = aux.InnerException;
            }
            vWriter.Flush();
            vWriter.Close();
        }
    }
}
