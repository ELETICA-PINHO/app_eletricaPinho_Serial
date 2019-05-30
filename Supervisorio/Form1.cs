using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace Supervisorio
{
    public partial class Form1 : Form
    {
        delegate void funcaoRecepcao();

        SerialPort SerialCom = new SerialPort();

      //  string bfRecebe = String.Empty;

      //  public delegate void Fdelegate(string a);


      //  public void recebe_Serial(string a)
       // {
            //txt_Recebe_Monitor_Serial.Text += a;
        //    txt_Recebe_Monitor_Serial.Text += RecepcaoSerial;

      //  }

        public Form1()
        {
            InitializeComponent();
            SerialCom.DataReceived += new SerialDataReceivedEventHandler(SerialCom_DataReceived);

            void SerialCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                //bfRecebe = SerialCom.ReadExisting();
              //  BeginInvoke(new Fdelegate(recebe_Serial), new object[] { bfRecebe });

                funcaoRecepcao recepcaodelegate = new funcaoRecepcao(RecepcaoSerial);
                Invoke(recepcaodelegate);


            }

            //configuração de serial 

            #region congiguração porta COM
            foreach (String str in SerialPort.GetPortNames())
            {
                comboBox_ComPort.Items.Add(str);
                comboBox_ComPort.Text = "COM1";
            }
            #endregion

            #region  congiguração porta Baud Rate

            comboBox_Baud_Rate.Text = "9600";

            #endregion

        }//fim form1

        ///  private void SerialCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //     {
        //     throw new NotImplementedException();
        //  }


        String chtxt = null, str = null;
        String comparacao = null;
        String novo = null;
        int aux;
        Boolean flag = false;

        public void RecepcaoSerial()
        {
            /*essa primeira parte 
             * recebe os dados enviados via 
             * Serial e exibe na aba Monitor Serial   
             * 
             */

            chtxt += SerialCom.ReadExisting();

            txt_Recebe_Monitor_Serial.Text += chtxt;
            novo += chtxt;
            comparacao += chtxt;
            str += chtxt;
            chtxt = null;
            

            if (txt_Recebe_Monitor_Serial.Text.Length > 200) //se dados no munitor serial passar de 770 e limpa automaticamente 
            {
                txt_Recebe_Monitor_Serial.Clear();
            }



            if (comparacao == "[LED1ON]")
            {
                picture_lamp1.Image = Supervisorio.Properties.Resources.lampada_on;
                lbl_lamp1.Text = "LAMP1-ON";
                comparacao = null;

            }

            if (comparacao == "[LED1OF]")
            {
                picture_lamp1.Image = Supervisorio.Properties.Resources.lampada_off;
                lbl_lamp1.Text = "LAMP1-OFF";
                comparacao = null;

            }

            if (comparacao == "[LED2ON]")
            {
                picture_lamp2.Image = Supervisorio.Properties.Resources.lampada_on;
                lbl_lamp2.Text = "LAMP2-ON";
                comparacao = null;

            }



            if (comparacao == "[LED2OF]")
            {
                picture_lamp2.Image = Supervisorio.Properties.Resources.lampada_off;
                lbl_lamp2.Text = "LAMP2-OFF";
                comparacao = null;

            }


            if (comparacao == "[LED3ON]")
            {
                picture_lamp3.Image = Supervisorio.Properties.Resources.lampada_on;
                lbl_lamp3.Text = "LAMP3-ON";
                comparacao = null;

            }



            if (comparacao == "[LED3OF]")
            {
                picture_lamp3.Image = Supervisorio.Properties.Resources.lampada_off;
                lbl_lamp3.Text = "LAMP3-OFF";
                comparacao = null;

            }


            if (comparacao == "[LED4ON]")
            {
                picture_lamp4.Image = Supervisorio.Properties.Resources.lampada_on;
                lbl_lamp4.Text = "LAMP4-ON";
                comparacao = null;

            }



            if (comparacao == "[LED4OF]")
            {
                picture_lamp4.Image = Supervisorio.Properties.Resources.lampada_off;
                lbl_lamp4.Text = "LAMP4-OFF";
                comparacao = null;

            }

            if (comparacao == "[LED5ON]")
            {
                picture_lamp5.Image = Supervisorio.Properties.Resources.lampada_on;
                lbl_lamp5.Text = "LAMP5-ON";
                comparacao = null;

            }



            if (comparacao == "[LED5OF]")
            {
                picture_lamp5.Image = Supervisorio.Properties.Resources.lampada_off;
                lbl_lamp5.Text = "LAMP5-OFF";
                comparacao = "";

            }

            if(novo.Length >= 8)
            {

                if (novo.Substring(0, 1) == "[" &&
               novo.Substring(1, 1) == "A" &&
               novo.Substring(2, 1) == "1" &&
               novo.Substring(7, 1) == "]")
                {
                    lbl_Sensor1Novo.Text = novo.Substring(3,4) ;
                    Barra_Sensor1.Value = int.Parse(novo.Substring(3, 4));

                    aux = int.Parse(novo.Substring(3,4));

                    float volts = 5 ;

                    volts = (aux * 5.0F) / 1023;
                    label4.Text = volts.ToString("F") + " Vots";


                    novo = "";
                    comparacao = "";

                }

            }
            

      

        }





        private void comboBox_ComPort_SelectedIndexChanged(object sender, EventArgs e)
       {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();

        }

        private void btn_sair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_conectar_Click(object sender, EventArgs e)
        {
            if (SerialCom.IsOpen == true) SerialCom.Close();

            SerialCom.PortName = comboBox_ComPort.Text;
            SerialCom.BaudRate = Int32.Parse(comboBox_Baud_Rate.Text);

            try
            {
                SerialCom.Open();
                btn_conectar.Enabled = false;
                btn_sair.Enabled = false;
                btn_desconectar.Enabled = true;
                painel_Status_conexao.BackColor = Color.Green;
                lbl_Status_conexao.Text = "CLOSE PORTE";
                comboBox_ComPort.Enabled = false;
                comboBox_Baud_Rate.Enabled = false;

            }
            catch
            {
                MessageBox.Show("Não foi possivel Conectar porta Selecionada!");
                btn_conectar.Enabled = true;
                btn_sair.Enabled = true;
                btn_desconectar.Enabled = true;
                comboBox_ComPort.Enabled = true;
                comboBox_Baud_Rate.Enabled = true;
                painel_Status_conexao.BackColor = Color.Red;
                lbl_Status_conexao.Text = "OPEN PORTE";
               
            }


        }

        private void btn_desconectar_Click(object sender, EventArgs e)
        {
            SerialCom.Close();
            btn_conectar.Enabled = true;
            btn_sair.Enabled = true;
            btn_desconectar.Enabled = false;
            comboBox_ComPort.Enabled = true;
            comboBox_Baud_Rate.Enabled = true;
            painel_Status_conexao.BackColor = Color.Red;
            lbl_Status_conexao.Text = "OPEN PORTE";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void txt_Envia_Monitor_Serial_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_Enviar_Serial_Click(object sender, EventArgs e)
        {
            if (SerialCom.IsOpen == true)
            {
                SerialCom.Write(txt_Envia_Monitor_Serial.Text + "\r\n");
                txt_Envia_Monitor_Serial.Text = "";

            }
        }

        private void btn_limpar_Monitor_Serial_Click(object sender, EventArgs e)
        {
            txt_Recebe_Monitor_Serial.Text = "";
        }

        private void comboBox_Baud_Rate_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Barra_Sensor2_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Sensor2_Click(object sender, EventArgs e)
        {

        }

        private void picture_lamp1_Click(object sender, EventArgs e)
        {
            SerialCom.Write("[BOTAO1]");

        }

        private void picture_lamp2_Click(object sender, EventArgs e)
        {
            SerialCom.Write("[BOTAO2]");
        }

        private void picture_lamp3_Click(object sender, EventArgs e)
        {
            SerialCom.Write("[BOTAO3]");
        }

        private void picture_lamp4_Click(object sender, EventArgs e)
        {
            SerialCom.Write("[BOTAO4]");
        }

        private void picture_lamp5_Click(object sender, EventArgs e)
        {
            SerialCom.Write("[BOTAO5]");
        }

        private void track_Pwm_Scroll(object sender, EventArgs e)
        {
            if (SerialCom.IsOpen == true)
            {
                int valorPWM = track_Pwm.Value;

                /*  5v >>>  1024
                 *  0V >>>  0 
                 */

                if(valorPWM == 0)
                {
                    SerialCom.Write("[PWM0%]");
                    lbl_Pwm.Text = "[PWM0%]";
                    comparacao = "";
                    novo = "";


                }


                if (valorPWM == 1)
                {
                    SerialCom.Write("[PWM25%]");
                    lbl_Pwm.Text = "[PWM25%]";
                    comparacao = "";
                    novo = "";


                }



                if (valorPWM == 2)
                {
                    SerialCom.Write("[PWM50%]");
                    lbl_Pwm.Text = "[PWM50%]";
                    comparacao = "";
                    novo = "";


                }


                if (valorPWM == 3)
                {
                    SerialCom.Write("[PWM75%]");
                    lbl_Pwm.Text = "[PWM75%]";
                    comparacao = "";
                    novo = "";


                }

                if (valorPWM == 4)
                {
                    SerialCom.Write("[PWM100%]");
                    lbl_Pwm.Text = "[PWM100%]";
                    comparacao = "";
                    novo = "";



                }






            }


        }

        private void lbl_Pwm_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://eletrica-pinho.blogspot.com/");
         
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        

            Process.Start("https://www.facebook.com/El%C3%A9trica-Pinho-1309240619096347/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCfwWEXZmV9y3PZhtJIJTAzA/videos?view_as=subscriber");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        

            Process.Start("https://www.instagram.com/eletricapinho/");

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        int conTimer = 0;
        
        private void timer1_Tick(object sender, EventArgs e)
        {

            /*
            conTimer++;
            if(conTimer >= 40)
            {
                if (flag == false)
                {
                    SerialCom.Write("[TIMEROK]");
                    conTimer = 0;

                }
                else
                {

                    SerialCom.Close();
                    btn_conectar.Enabled = true;
                    btn_sair.Enabled = true;
                    btn_desconectar.Enabled = false;
                    comboBox_ComPort.Enabled = true;
                    comboBox_Baud_Rate.Enabled = true;
                    painel_Status_conexao.BackColor = Color.Red;
                    lbl_Status_conexao.Text = "OPEN PORTE";
                    MessageBox.Show("Erro de conexão");
                    conTimer = 0;

                }

            }
            else
            {
                if(conTimer == 20)
                {
                    SerialCom.Write("[TIMEROK]");
                    flag = true;


                }

            }





    */

          //  SerialCom.Write("[SENSOR]");
           comparacao = "";
           novo = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {
          
        }

        private void lbl_Sensor1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Sensor1Novo_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

           if(SerialCom.IsOpen == true)
            {
                novo = "";
                comparacao = "";
                SerialCom.Write("[SENSOR]");
               


            }
           

        }

        private void txt_Recebe_Monitor_Serial_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
