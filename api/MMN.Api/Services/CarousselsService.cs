using Mapster;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Negocio.Negocio;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface ICarouselsService
{
    IEnumerable<Carrossel> GetCarousels();
    Carrossel GetCarouselById(int id);
    Carrossel AddCarousel(Carrossel carrossel);
    Carrossel UpdateCarousel(Carrossel carrossel);
    bool DeleteCarousel(int id);
}

public class CarouselsService : ICarouselsService
{
    private readonly IDbConnection _dbConnection;
    private readonly ICarrosselNegocio _carrosselNegocio;

    public CarouselsService(IDbConnection dbConnection, ICarrosselNegocio carrosselNegocio)
    {
        _dbConnection = dbConnection;
        _carrosselNegocio = carrosselNegocio;
    }

    public Carrossel AddCarousel(Carrossel carrossel)
    {
        _carrosselNegocio.Insert(carrossel.Adapt<CarrosselViewModel>());
        
        return carrossel;
    }

    public bool DeleteCarousel(int id)
    {
        var carrossel = GetCarouselById(id);
        
        if (carrossel != null)
        {
            _carrosselNegocio.Delete(carrossel.IdCarrossel);
            return true;
        }
        
        return false;
    }

    public Carrossel GetCarouselById(int id)
    {
        return _carrosselNegocio.FirstNoTracking(x => x.IdCarrossel == id).Adapt<Carrossel>();
    }

    public IEnumerable<Carrossel> GetCarousels()
    {
        return _carrosselNegocio.GetAllNoTracking().Adapt<IEnumerable<Carrossel>>().OrderBy(x => x.Ativo).ThenBy(x => x.Posicao).ThenBy(x => x.OrdemExibicao);
    }

    public Carrossel UpdateCarousel(Carrossel carrossel)
    {
        var c = carrossel.Adapt<CarrosselViewModel>();
        _carrosselNegocio.Update(c);
        
        return carrossel;
    }
}
