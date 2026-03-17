using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Negocio.Negocio
{ 
    public class CarrosselNegocio : BaseNegocio<CarrosselViewModel, Carrossel>, ICarrosselNegocio
    {
        private readonly ICarrosselRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CarrosselNegocio(ICarrosselRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
