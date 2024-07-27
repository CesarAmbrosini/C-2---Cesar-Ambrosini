using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace Presentacion
{
    public partial class FrmAltaArticulo : Form
    {
        private Articulo articulo = null;

        private OpenFileDialog archivo = null;
        public FrmAltaArticulo()
        {
            InitializeComponent();
            Text = "Agregar articulo";
        }

        public FrmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
        }

        private void FrmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio negocioMarca = new MarcaNegocio(); 
            CategoriaNegocio negocioCategoria = new CategoriaNegocio();

            try
            {
                cbxMarca.DataSource = negocioMarca.listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";
                cbxCategoria.DataSource = negocioCategoria.listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    tbxCodigo.Text = articulo.Codigo;
                    tbxNombre.Text = articulo.Nombre;
                    tbxDescripcion.Text = articulo.Descripcion;
                    tbxPrecio.Text = articulo.Precio.ToString();
                    tbxUrlimagen.Text = articulo.UrlImagen;
                    cbxCategoria.SelectedValue = articulo.Categoria.Id;
                    cbxMarca.SelectedValue = articulo.Marca.Id;
                    cargarImagen(articulo.UrlImagen);
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar.");
            }

        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
                ArticuloNegocio negocio = new ArticuloNegocio();

            Helper validarNum = new Helper();

            if(tbxCodigo.Text == "" || tbxNombre.Text == "" || tbxPrecio.Text == "")
            {
                MessageBox.Show("Los campos Codigo, Nombre y Precio son obligatorios.");
                return;
            }

            if (!(validarNum.soloNumeros(tbxPrecio.Text)))
            {
                MessageBox.Show("Por favor, ingrese solo numeros en la casilla de Precio.");
                return;
            }


            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = tbxCodigo.Text;
                articulo.Nombre = tbxNombre.Text;
                articulo.Descripcion = tbxDescripcion.Text;
                articulo.Precio = decimal.Parse(tbxPrecio.Text);   
                articulo.UrlImagen = tbxUrlimagen.Text; 
                articulo.Categoria = (Categoria)cbxCategoria.SelectedItem;    
                articulo.Marca = (Marca)cbxMarca.SelectedItem;

                if (articulo.Id == 0)
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente.");
                }
                else
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente.");
                }

                //if(archivo != null && !(tbxUrlimagen.Text.ToUpper().Contains("HTTP")))
                //    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Siga participando.");
            }
        }

        private void tbxUrlimagen_Leave(object sender, EventArgs e)
        {
            try
            {
                cargarImagen(tbxUrlimagen.Text); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error con la imagen.");
            }
        }

        public void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://ih1.redbubble.net/image.1823351869.7092/raf,360x360,075,t,fafafa:ca443f4786.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
                if(archivo.ShowDialog() == DialogResult.OK)
                {
                    tbxUrlimagen.Text = archivo.FileName;
                    cargarImagen(archivo.FileName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No me quemeee");
            }
        }
    }
}
