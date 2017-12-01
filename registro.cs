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
[Activity(Label = "registro",Theme = "@style/MyTheme", NoHistory=true)]
	public class registro: ActionBarActivity
	{
		private SupportToolbar mToolBar;
		Button btnConfirmar;
		EditText txt_nombre;
		EditText txt_apellidos;
		EditText txt_correo;
		EditText txt_pass;
		EditText txt_cPass;
		ProgressBar pb;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Registro);
			// Create your application here
			mToolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
			pb = FindViewById<ProgressBar>(Resource.Id.pb);
			btnConfirmar = FindViewById<Button>(Resource.Id.btnConfirmar);
			txt_nombre = FindViewById<EditText>(Resource.Id.txt_nombre);
			txt_apellidos = FindViewById<EditText>(Resource.Id.txt_apellidos);
			txt_correo = FindViewById<EditText>(Resource.Id.txt_correo);
			txt_pass = FindViewById<EditText>(Resource.Id.txt_password);
			txt_cPass = FindViewById<EditText>(Resource.Id.txt_Cpassword);
			pb.Visibility = ViewStates.Invisible;
			// Create your application here
			SetSupportActionBar(mToolBar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetHomeButtonEnabled(true);


			btnConfirmar.Click += delegate
			{
				try
				{
					if (txt_nombre.Text != string.Empty && txt_pass.Text != string.Empty && txt_correo.Text != string.Empty && txt_apellidos.Text != string.Empty)
					{
						if (txt_pass.Text == txt_cPass.Text)
						{
                            Servicio.Servicio sw = new Servicio.Servicio();
							pb.Visibility = ViewStates.Visible;
							btnConfirmar.Enabled = false;
							sw.registrarUsuarioCompleted += Sw_RegistrarUsuarioCompleted;
							sw.registrarUsuarioAsync(txt_nombre.Text, txt_apellidos.Text, txt_correo.Text, txt_pass.Text);

						}
						else
						{
							Toast.MakeText(this, "La confirmacion de la contraseña no corresponde", ToastLength.Long).Show();
						}
					}
					else
					{
						Toast.MakeText(this, "Por favor ingresa todos los datos solicitados.", ToastLength.Short).Show();
					}
				}
				catch (Exception ex)
				{
					Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
				}
			};
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					var hom = new Intent(this, typeof(MainActivity));
					StartActivity(hom);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
		void Sw_RegistrarUsuarioCompleted(object sender, Servicio.registrarUsuarioCompletedEventArgs e)
		{
			if (e.Result == "Registrado")
			{
				Toast.MakeText(this, e.Result, ToastLength.Short).Show();
				var log = new Intent(this, typeof(MainActivity));
				txt_nombre.Text = "";
				txt_pass.Text = "";
				txt_correo.Text = "";
				txt_apellidos.Text = "";
				txt_cPass.Text = "";
				StartActivity(log);
			}
			else
			{
				Toast.MakeText(this, "No se ha registrado el usuario" + e.Result, ToastLength.Short).Show();
				txt_nombre.Text = "";
				txt_pass.Text = "";
				txt_correo.Text = "";
				txt_apellidos.Text = "";
				txt_cPass.Text = "";
			}
			pb.Visibility = ViewStates.Invisible;
			btnConfirmar.Enabled = true;
		}
	}
}
