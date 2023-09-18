using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepositorio.Consultar();

                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();

                return _mapper.Map<List<ProductoDTO>>(listaProductos.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));

                if(productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var ProductoEncontrado = await _productoRepositorio.Obtener(p =>
                p.IdProducto == productoModelo.IdProducto
                );

                if(ProductoEncontrado == null)
                {
                    throw new TaskCanceledException("No se encontro el producto a editar");
                }

                ProductoEncontrado.Nombre = productoModelo.Nombre;
                ProductoEncontrado.IdCategoria = productoModelo.IdCategoria;
                ProductoEncontrado.Stock = productoModelo.Stock;
                ProductoEncontrado.Precio = productoModelo.Precio;
                ProductoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(ProductoEncontrado);

                if(!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el producto");
                }


                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.Obtener(p =>
                p.IdProducto == id
                );

                if(productoEncontrado == null)
                {
                    throw new TaskCanceledException("No se encontro el producto a eliminar");
                }

                bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);

                if(!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar el producto");
                }

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

    }
}
