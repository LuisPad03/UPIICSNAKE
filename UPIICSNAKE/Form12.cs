using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPIICSNAKE
{
    public partial class Form12 : Form
    {
        List<PictureBox> Lista = new List<PictureBox>();
        int TamanioPiezaPrincipal = 26, tiempo = 10;
        PictureBox Comida = new PictureBox();
        String Direccion = "right"; // direccion de la vivorita

        public Form12()
        {
            InitializeComponent();
            IniciarJuego();
        }

        public void IniciarJuego()
        {
            
            tiempo = 10;
            Direccion = "right";
            timer1.Interval = 200/*135*/;
            Lista = new List<PictureBox>();

            for (int i=1; 0<=i;i--) // Piezas iniciales
            {
                CrearSnake(Lista, this, (i * TamanioPiezaPrincipal) + 70, 80);

            }
            CrearComida();

        }

        public void CrearSnake (List<PictureBox> ListaPelota, Form formulario, int posicionx, int posiciony)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new Point(posicionx, posiciony);
            pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("CPO");
            pb.BackColor = Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            ListaPelota.Add(pb);
            formulario.Controls.Add(pb);
        } 

        private void CrearComida()
        {
            // crea la imagen de la comida
            Random rnd = new Random();
            int enterox = rnd.Next(1, this.Width - TamanioPiezaPrincipal - 20);
            int enteroy = rnd.Next(1, 400 - TamanioPiezaPrincipal - 40);
            //
            PictureBox pb = new PictureBox();
            pb.Location = new Point(enterox, enteroy);
            pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("comida");
            pb.BackColor = Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            Comida = pb;
            this.Controls.Add(pb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nx = Lista[0].Location.X;
            int ny = Lista[0].Location.Y;
            // cabeza de la serpiente
            Lista[0].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("CAB" + Direccion);

            for (int i = Lista.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    if (Direccion == "right") nx = nx + TamanioPiezaPrincipal;
                    else if (Direccion == "left") nx = nx - TamanioPiezaPrincipal;
                    else if (Direccion == "up") ny = ny - TamanioPiezaPrincipal;
                    else if (Direccion == "down") ny = ny + TamanioPiezaPrincipal;
                    Lista[0].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("CAB" + Direccion);
                    Lista[0].Location = new Point(nx, ny);
                }
                else
                {   //intercambio de seguimiento
                    Lista[i].Location = new Point((Lista[i - 1].Location.X), (Lista[i].Location.Y));
                    Lista[i].Location = new Point(Lista[i].Location.X, Lista[i - 1].Location.Y);
                }
            }
                // choque con comida
            for (int contarPiezas = 1; contarPiezas < Lista.Count; contarPiezas++) 
                {
                    if (Lista[contarPiezas].Bounds.IntersectsWith(Comida.Bounds)) // detecta los choques con la comida
                    {
                         this.Controls.Remove(Comida);  // al chocar con una comida la elimia
                         tiempo = Convert.ToInt32(timer1.Interval); // amenta la velocidad
                         if (tiempo > 10) { timer1.Interval = tiempo - 10; }
                         lblPuntos.Text = (Convert.ToInt32(lblPuntos.Text) + 1).ToString();
                         CrearComida();
                         CrearSnake(Lista, this, Lista[Lista.Count - 1].Location.X * TamanioPiezaPrincipal, 0);
                    }
                    
                }
                // choque con paredes
            if ((Lista[0].Location.X >= 590)/*chocar a la derecha*/ || (Lista[0].Location.X < 0)/* chocar a la izquierda*/ || (Lista[0].Location.Y > 350) /*chocar hacea abajo*/ || (Lista[0].Location.Y < 2) /*chocar hacia arriba*/)
            {
                ReiniciarJuego();
            }

            for (int contarPiezas = 1; contarPiezas < Lista.Count; contarPiezas++) 
            {                                                                  
                if (Lista[0].Bounds.IntersectsWith(Lista[contarPiezas].Bounds))
                {
                    ReiniciarJuego();
                }
            }
        }

        public void ReiniciarJuego()
        {
            foreach (PictureBox Serpiente in Lista) { this.Controls.Remove(Serpiente); }
            this.Controls.Remove(Comida);
            IniciarJuego();
            timer1.Enabled = false;
            lblPuntos.Text = "0";
        }

        private void Form12_KeyPress(object sender, KeyPressEventArgs e)
        { 
        }

        private void MoverPieza(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                timer1.Enabled = true;
               
            }
            if (e.KeyData == Keys.Space)
            {
                timer1.Stop();
            }

            Direccion = ((e.KeyCode & Keys.Up) == Keys.Up) ? "up" : Direccion;
            Direccion = ((e.KeyCode & Keys.Down) == Keys.Down) ? "down" : Direccion;
            Direccion = ((e.KeyCode & Keys.Left) == Keys.Left) ? "left" : Direccion;
            Direccion = ((e.KeyCode & Keys.Right) == Keys.Right) ? "right" : Direccion;

        }
    }
}
