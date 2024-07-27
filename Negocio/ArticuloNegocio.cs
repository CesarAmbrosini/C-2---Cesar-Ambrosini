using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();  

            try
            {
                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion as Marca, C.Descripcion  as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca AND C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo auxArticulo = new Articulo();

                    auxArticulo.Id = (int)datos.Lector["Id"];
                    auxArticulo.Codigo = (string)datos.Lector["Codigo"];
                    auxArticulo.Nombre = (string)datos.Lector["Nombre"];
                    auxArticulo.Descripcion = (string)datos.Lector["Descripcion"];
                    auxArticulo.Marca = new Marca();
                    auxArticulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    auxArticulo.Marca.Descripcion = (string)datos.Lector["Marca"];
                    auxArticulo.Categoria = new Categoria();    
                    auxArticulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    auxArticulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    if(!(datos.Lector["ImagenUrl"] is DBNull))
                        auxArticulo.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    auxArticulo.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(auxArticulo);
                }


                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Url, @Precio)");
                datos.setearParametro("@Codigo", articulo.Codigo);
                datos.setearParametro("@Nombre", articulo.Nombre);
                datos.setearParametro("@Descripcion", articulo.Descripcion);
                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@Url", articulo.UrlImagen);
                datos.setearParametro("@Precio", articulo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion(); 
            }
        }
        
        public void modificar(Articulo modificar)
        {
            AccesoDatos datos = new AccesoDatos();  
            
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @Url, Precio = @Precio where Id = @Id");
                datos.setearParametro("@Codigo", modificar.Codigo);
                datos.setearParametro("@Nombre", modificar.Nombre);
                datos.setearParametro("@Descripcion", modificar.Descripcion);
                datos.setearParametro("@IdMarca", modificar.Marca.Id);
                datos.setearParametro("@IdCategoria", modificar.Categoria.Id);
                datos.setearParametro("@Url", modificar.UrlImagen);
                datos.setearParametro("@Precio", modificar.Precio);
                datos.setearParametro("@Id", modificar.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarFisico(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();  

            try
            {
                datos.setearConsulta("delete ARTICULOS where Id = @Id");
                datos.setearParametro("@Id", articulo.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            { 
                datos.cerrarConexion(); 
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion as Marca, C.Descripcion  as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca AND C.Id = A.IdCategoria And ";
                
                if(campo == "Precio")
                {
                    if (criterio == "Mayor a")
                    {
                        consulta += "Precio > " + filtro;
                    }
                    else if (criterio == "Menor a")
                    {
                        consulta += "Precio < " + filtro;
                    }
                    else
                    {
                        consulta += "Precio = " + filtro;
                    }
                }
                else if (campo == "Marca")
                {
                    if (criterio == "Comienza con")
                    {
                        consulta += "M.Descripcion like '" + filtro + "%'";
                    }
                    else if (criterio == "Termina con")
                    {
                        consulta += "M.Descripcion like '%" + filtro + "'";
                    }
                    else
                    {
                        consulta += "M.Descripcion like '%" + filtro + "%'";
                    }
                }
                else
                {
                    if (criterio == "Comienza con")
                    {
                        consulta += "C.Descripcion like '" + filtro + "%'";
                    }
                    else if (criterio == "Termina con")
                    {
                        consulta += "C.Descripcion like '%" + filtro + "'";
                    }
                    else
                    {
                        consulta += "C.Descripcion like '%" + filtro + "%'";
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo auxArticulo = new Articulo();

                    auxArticulo.Id = (int)datos.Lector["Id"];
                    auxArticulo.Codigo = (string)datos.Lector["Codigo"];
                    auxArticulo.Nombre = (string)datos.Lector["Nombre"];
                    auxArticulo.Descripcion = (string)datos.Lector["Descripcion"];
                    auxArticulo.Marca = new Marca();
                    auxArticulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    auxArticulo.Marca.Descripcion = (string)datos.Lector["Marca"];
                    auxArticulo.Categoria = new Categoria();
                    auxArticulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    auxArticulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        auxArticulo.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    auxArticulo.Precio = (decimal)datos.Lector["Precio"];

                    listaArticulos.Add(auxArticulo);
                }

                return listaArticulos;  
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        
    }
}
