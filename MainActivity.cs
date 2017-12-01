using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Xml.Serialization;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace AppService
{
	[Activity(Label = "AppService", MainLauncher = true, Icon = "@drawable/logo")]
	public class MainActivity : Activity
	{
		public EditText txtCorreo;
		public EditText txtPass;
		public Button btnAcceso;
		public Button btnRegistro;
		public ProgressBar pb;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			btnAcceso = FindViewById<Button>(Resource.Id.btnLogin);
			btnRegistro = FindViewById<Button>(Resource.Id.btnRegistro);
			txtCorreo = FindViewById<EditText>(Resource.Id.txtUser);
			txtPass = FindViewById<EditText>(Resource.Id.txtPassword);
			pb = FindViewById<ProgressBar>(Resource.Id.pbBar);
			pb.Visibility = ViewStates.Invisible;


			btnAcceso.Click += delegate
			{
                    //var inicio = new Intent(this, typeof(inicio));
                    //StartActivity(inicio);
                    if (txtPass.Text != string.Empty && txtCorreo.Text != string.Empty)
                    {
                        //var home = new Intent(this, typeof(MainActivity));
                        //StartActivity(home);

                    Servicio.Servicio service = new Servicio.Servicio();
                        service.AccederCompleted += Service_AccederCompleted;
                        service.AccederAsync(txtCorreo.Text, txtPass.Text);
                        pb.Visibility = ViewStates.Visible;
                        btnAcceso.Enabled = false;
                        btnRegistro.Enabled = false;
                    }
                    else
                    {
                        Toast.MakeText(this, "Ingresa correctamente los datos para acceder.", ToastLength.Short).Show();
                    }
			};
			btnRegistro.Click += delegate
			{
				var registro = new Intent(this, typeof(registro));
				StartActivity(registro);
			};
		}
		void Service_AccederCompleted(object sender, Servicio.AccederCompletedEventArgs e)
		{
			if (!e.Result)
			{
				Toast.MakeText(this, "Error de usuario", ToastLength.Short).Show();
				pb.Visibility = ViewStates.Invisible;
				btnAcceso.Enabled = true;
				btnRegistro.Enabled = true;
			}
			else
			{
				User(txtCorreo.Text, txtPass.Text);
				var home = new Intent(this, typeof(Inicio));
				StartActivity(home);
				pb.Visibility = ViewStates.Invisible;
				btnAcceso.Enabled = true;
				btnRegistro.Enabled = true;
			}
		}
		public void User(string correo, string pass)
		{
			Use us = new Use();
			us.Correo = correo;
			us.Password = pass;
			XmlSerializer serializar = new XmlSerializer(typeof(Use));
			string Ruta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			StreamWriter Escritura = new StreamWriter(Ruta + "usuario.xml");
			serializar.Serialize(Escritura, us);
			Escritura.Close();
			Toast.MakeText(this, us.Correo + " " + us.Password, ToastLength.Short).Show();
		}
	}
}

