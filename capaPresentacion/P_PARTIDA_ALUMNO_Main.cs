﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using capaEntidad;
using capaNegocio;
using capaDatos;

namespace capaPresentacion
{
    public partial class P_PARTIDA_ALUMNO_Main : Form
    {
        public P_PARTIDA_ALUMNO_Main(E_focusedBible Configuracion)
        {
            objEntidad = Configuracion;

            InitializeComponent();
        }

        E_focusedBible objEntidad = new E_focusedBible();
        E_Alumnos objEntidadAlumno = new E_Alumnos();
        N_focusedBible objNego = new N_focusedBible();
        N_AlumnoPartida objNegoAlumno = new N_AlumnoPartida();
        N_Listener objNegoListener = new N_Listener();
        P_GameSettings GameSettings;
        DataSet ds;
        DataTable dt;

        public int countDown = 5;
        public bool lockStart = true;

        public string difficulty;
        public string queryPorDificultad;
        public string[] catEvangelios_yOtros = new string[10];
        public string[] catLibro = new string[66];
        public string catNuevoAntiguo;
        public int numRounds;
        public int time2Answer;


        private void P_Debate_Main_Load(object sender, EventArgs e)
        {
            SetDoubleBuffered(tableLayoutPanel6);
            SetDoubleBuffered(tableLayoutPanel7);
            SetDoubleBuffered(tableLayoutPanel8);
            SetDoubleBuffered(tableLayoutPanel10);
            SetDoubleBuffered(tableLayoutPanel8);
            SetDoubleBuffered(tableLayoutPanel19);
            SetDoubleBuffered(tableLayoutPanel20);
            SetDoubleBuffered(tableLayoutPanel21);

            // Actualizar Estado Jugador en base de datos
            objEntidadAlumno.NombreUsuario = E_Usuario.Nombreusuario;
            objEntidadAlumno.Estado = "False";
            objEntidadAlumno.Terminado = "False";
            // actualiza y en caso de no existir lo inserta
            
            if (objNegoAlumno.N_Editar(objEntidadAlumno) == 0)
            {
                objNegoAlumno.N_Insertar(objEntidadAlumno);
            }               


            if (startStopGame("start"))
            {
                lockStart = true;
                lab_countDown2Srart.Text = "Se Acaba de quedar Fuera de la PARTIDA";
                tbx_codigoPartida.Enabled = false;
            }
            else
            {
                lockStart = false;
            }
            
            this.BackgroundImage = Properties.Resources.Fondo_Debate_Main_ConTextBox;
            BackgroundImageLayout = ImageLayout.Stretch;
        }


        private bool startStopGame(string comando)
        {
            ds = objNegoListener.N_Listener_Comando(1);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Comando"].ToString() == comando)
                {
                    return true;
                }
            }

            return false;
        }

        
        private void btn_goToMain_Click(object sender, EventArgs e)
        {
            Form existe = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "P_Main").SingleOrDefault<Form>();

            if (existe != null) // para saber si el formulario principal existe
            {
                this.AddOwnedForm(existe); //indica que este va a ser el papa del form P_Main
                existe.Close(); // cerrar ventana principal
            }

            P_Main PMain = new P_Main(objEntidad);
            this.AddOwnedForm(PMain); //indica que este va a ser el papa del form P_Main

            PMain.Show();
            this.RemoveOwnedForm(PMain); //indica que este va a dejar de ser el papa del form P_Main
            this.Close();
        }


        private void btn_goToMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27) //si la tecla pesionada es igual a ESC (27)
            {
                this.DialogResult = DialogResult.OK; //cierra el esta ventana y deja vista la ventana Main
            }
        }

        private void OpenSettings()
        {

            try
            {   // para saber si el formulario existe, o sea si está abierto o cerrado
                Form existe = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "P_Login").SingleOrDefault<Form>();
                Form existe2 = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "P_focusedBible_Debate").SingleOrDefault<Form>();

                if (existe != null)
                {
                    existe.Close();
                }

                if (existe2 != null) // para cerrar el juego, en caso de haberse iniciado
                {
                    existe2.Close();
                }

                P_Login login = new P_Login();
                login.reOpened++;
                this.Hide();
                login.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Algo salió mal, Favor intentarlo nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private void Btn_Settings_MouseEnter(object sender, EventArgs e)
        {
            objEntidad.reproducirSonidoBoton("button.wav", false);
            Btn_Settings.Image = Properties.Resources.Focused_bible_landing_02_MOUSE_ENTER;
        }

        private void Btn_Settings_MouseLeave(object sender, EventArgs e)
        {
            Btn_Settings.Image = Properties.Resources.Focused_bible_landing_02;
        }

        private void Btn_Settings_Click(object sender, EventArgs e)
        {
            OpenGameSettings();
        }

        private void OpenGameSettings()
        {
            Change_Settings();

            // para saber si el formulario existe, o sea, si está abierto o cerrado
            Form existe = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "P_GameSettings").SingleOrDefault<Form>();
            Form existe2 = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "P_Main").SingleOrDefault<Form>();

            if (existe != null)
            {
                existe.Close();
                existe.Dispose();
                GC.Collect();
            }

            GameSettings = new P_GameSettings(objEntidad);
            existe2.Hide();
            //this.Hide();
            GameSettings.ShowDialog();
        }


        public void Change_Settings()
        {
        }


        // Las siguentes dos funciones son para
        //evitar los problemas de Buffer por tener layouts transparentes
        #region .. Double Buffered function ..
        public static void SetDoubleBuffered(Control c)
        {
            if (SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        #endregion
        #region .. code for Flucuring ..

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #endregion



        private void btn_goToMain_MouseEnter(object sender, EventArgs e)
        {
            objEntidad.reproducirSonidoBoton("button.wav", false);
            btn_goToMain.BackgroundImage = Properties.Resources.Focused_bible_SOLO_07_MouseEnter;
        }

        private void btn_goToMain_MouseLeave(object sender, EventArgs e)
        {
            btn_goToMain.BackgroundImage = Properties.Resources.Focused_bible_SOLO_07;
        }

        private void tbx_codigoPartida_TextChanged(object sender, EventArgs e)
        {
            if (tbx_codigoPartida.Text.Length == 4)
            {
                ds = objNegoListener.N_Listener_Comando(1);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["codigoProfe"].ToString() == tbx_codigoPartida.Text)
                    {
                        objEntidadAlumno.Estado = "True";
                        objNegoAlumno.N_Editar(objEntidadAlumno);

                        MessageBox.Show("Codigo Valido!");
                    }
                    else
                    {
                        objEntidadAlumno.Estado = "False";
                        objNegoAlumno.N_Editar(objEntidadAlumno);
                        MessageBox.Show("Codigo No Valido!");
                    }
                }
            }
        }


        private void P_PARTIDA_ALUMNO_Main_Activated(object sender, EventArgs e)
        {
            if (lockStart == false)
            {
                if (startStopGame("start"))
                {
                    timer2Start.Start();
                }
            }
        }

        private void timer2Start_Tick(object sender, EventArgs e)
        {
            if (countDown > 0)
            {
                countDown--;
                lab_countDown2Srart.Text = countDown.ToString();
            }
            else
            {
                timer2Start.Stop();

                ds = objNegoListener.N_Listener_Comando(1);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["codigoProfe"].ToString() == tbx_codigoPartida.Text)
                    {
                        this.Hide();
                        P_focusedBible_SOLO_y_PARTIDA soloMain = new P_focusedBible_SOLO_y_PARTIDA(objEntidad);
                        soloMain.Show();
                    }
                    else
                    {
                        lab_countDown2Srart.Text = "Se Acaba de quedar Fuera de la PARTIDA";
                        tbx_codigoPartida.Enabled = false;
                    }
                }
            }
        }
    }
}
