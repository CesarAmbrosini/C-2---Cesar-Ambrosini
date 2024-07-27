using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;


namespace Presentacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCampo.Items.Add("Precio");
            cbxCampo.Items.Add("Marca");
            cbxCampo.Items.Add("Categoria");
            Text = "Gestion de Articulos";
        }

        private List<Articulo> listaArticulo;

        private void cargar()
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar.");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaArticulo ventanaAltaArt = new FrmAltaArticulo();
            ventanaAltaArt.ShowDialog();
            cargar();
        }
        
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;    
            FrmAltaArticulo frmAltaArticulo = new FrmAltaArticulo(articulo);
            frmAltaArticulo.ShowDialog();
            cargar();
        }
        
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Seguro que desea eliminar este articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            try
            {
                if (resultado == DialogResult.Yes)
                {
                    Articulo articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    ArticuloNegocio articuloNegocio = new ArticuloNegocio();

                    articuloNegocio.eliminarFisico(articulo);
                    MessageBox.Show("Eliminado exitosamente.");
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error, vuelva mas tarde.");
            }
        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(articuloSeleccionado.UrlImagen);
            }
        }

        public void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulos.Load("https://ih1.redbubble.net/image.1823351869.7092/raf,360x360,075,t,fafafa:ca443f4786.jpg");
            }
        }

        private bool validarFiltro()
        {
            if(cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor elegir campo!!");
                return true;   
            }
            if (cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor elegir criterio!!");
                return true;
            }
            if (cbxCampo.SelectedItem.ToString() == "Precio")
            {
                Helper validarNum = new Helper();

                if(tbxFiltroAvanzado.Text != "" && tbxFiltroAvanzado.Text != null)
                {
                    if (!(validarNum.soloNumeros(tbxFiltroAvanzado.Text)))
                    {
                        MessageBox.Show("Por favor ingrese solo numeros.");
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Por favor rellene el filtro bien rellenadito y con numeritos solamente.");
                    return true;
                }
            } 
            return false;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                if (validarFiltro())
                    return;

                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = tbxFiltroAvanzado.Text;

                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar");
            }

        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["UrlImagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = tbxFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
           
            if(opcion == "Precio")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor a");
                cbxCriterio.Items.Add("Menor a");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }
    }
}
