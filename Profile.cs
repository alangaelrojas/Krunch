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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

using Com.Bumptech.Glide;

namespace AppService
{
[Activity(Label = "Profile", Theme = "@style/MyTheme", NoHistory=true)]
	public class Profile : ActionBarActivity
	{
		private SupportToolbar mToolBar;
		ImageView img;
		TextView txtApellidos;
		TextView txtNombre;
		TextView txtCorreo;
		TextView txtSaldo;
		Button btnCerrarSesion;
        String urll;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Perfil);
			// Create your application here

			mToolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
			img = FindViewById<ImageView>(Resource.Id.imgAvatar);
			txtNombre = FindViewById<TextView>(Resource.Id.txt_nombre);
			txtApellidos = FindViewById<TextView>(Resource.Id.txt_apellidos);
			txtCorreo = FindViewById<TextView>(Resource.Id.txt_correo);
			txtSaldo = FindViewById<TextView>(Resource.Id.txt_saldo);
			btnCerrarSesion = FindViewById<Button>(Resource.Id.btnCerrarSesion);
			// Create your application here
			SetSupportActionBar(mToolBar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetHomeButtonEnabled(true);
			verDatos();

            btnCerrarSesion.Click += delegate {
				var hom = new Intent(this, typeof(MainActivity));
				StartActivity(hom);

			};
		}
		public void verDatos()
		{
			try
			{
				Use us = new Use();
                Servicio.Servicio sw = new Servicio.Servicio();
				XmlSerializer seriarlizar = new XmlSerializer(typeof(Use));
				string ruta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				StreamReader Lectura = new StreamReader(ruta + "usuario.xml");
				Use b = (Use)seriarlizar.Deserialize(Lectura);
				Lectura.Close();
				sw.InfoUsuarioCompleted += Sw_InfoUsuarioCompleted;
				sw.InfoUsuarioAsync(b.Correo, b.Password);
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
				Toast.MakeText(this, "Inicia Sesion Nuevamente", ToastLength.Short).Show();
			}
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
                    var hom = new Intent(this, typeof(Inicio));
					StartActivity(hom);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
		void Sw_InfoUsuarioCompleted(object sender, Servicio.InfoUsuarioCompletedEventArgs e)
		{
			DataSet conjunto = new DataSet();
			DataRow renglon;
			try
			{
				conjunto = e.Result;
				renglon = conjunto.Tables["comidas"].Rows[0];
				txtNombre.Text = (renglon["Nombre"]).ToString();
				txtApellidos.Text = (renglon["Apellidos"]).ToString();
				txtCorreo.Text = (renglon["Correo"]).ToString();
                urll = (renglon["url"]).ToString();
				Glide.With(this)
			 .Load(urll)
			.Transform(new CircleTransform(this))
			.Into(img);
				if ((renglon["Saldo"]).ToString() == "")
				{
					txtSaldo.Text = "$ 0";
				}
				else
				{
					txtSaldo.Text = "$ " + (renglon["Saldo"]).ToString();
				}

			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();

			}
		}
	}
}
