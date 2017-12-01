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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace AppService
{
	[Activity(Label = "Inicio", Theme = "@style/MyTheme")]
	public class Inicio : ActionBarActivity
	{
		/**Lista de datos en la pantalla principal**/
		List<cls_datos> elem = new List<cls_datos>();
		List<cls_datos> car = new List<cls_datos>();

		public double saldo = 0;//saldo del usuario
								/*******************************************/
		TextView lbl_credito;
		TextView lbl_sub_total;
		int sub;

		private SupportToolbar mToolBar;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		private ListView mRightDrawer;
		private MyActionBarDrawerToggle mDrawerToggle;
		/*Elementos del menu derecho**/
		ArrayAdapter mLeftAdapter;
		List<string> mLeftItems = new List<string>();
		/*Elementos para el carrito*/
		//ArrayAdapter mRightAdapter;
		List<string> mRightItems = new List<string>();
		ListView lwElem;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Inicio);

			mToolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);/*lista del menu*/
			mRightDrawer = FindViewById<ListView>(Resource.Id.right_drawer);
			lbl_credito = FindViewById<TextView>(Resource.Id.lbl_credito);
			lbl_sub_total = FindViewById<TextView>(Resource.Id.lbl_subtotal);
			//fin de los elementos referenciados
			mLeftDrawer.Tag = 0;
			mRightDrawer.Tag = 1;
			SetSupportActionBar(mToolBar);
			lbl_sub_total.Text = "Sub_Total: $0";
			//llamado para mostrar datos desde el web service
			verDatos();
			GC.Collect();
			//-----------------------------------------------
			/*Opciones de la lista Menu*/
			mLeftItems.Add("Inicio");
			mLeftItems.Add("Promociones");
			mLeftItems.Add("Perfil");
			mLeftItems.Add("Historial");
			mLeftItems.Add("Acerca de");
			mLeftItems.Add("Salir");
            //-----------------------------------------------
            //var listt=Android.Resource.Layout.SimpleListItem1;

            mLeftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, mLeftItems);
            //mLeftAdapter = new ArrayAdapter(this, listt, mLeftItems);
            mLeftDrawer.Adapter = mLeftAdapter;


			//-----------------------------------------------
			//probablemente aqui agrege algo de codigo//
			llenarList();

			//eventos de las opciones del menu
			mRightDrawer.Adapter = new adapter_carrito(this, car);
			/********************************/
			mDrawerToggle = new MyActionBarDrawerToggle(this, mDrawerLayout, Resource.String.openDrawer, Resource.String.closeDrawer);
			mDrawerLayout.SetDrawerListener(mDrawerToggle);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			mDrawerToggle.SyncState();

			//-----------------------------------------------
			if (bundle != null)
			{
				if (bundle.GetString("DrawerState") == "Opened")
				{
					SupportActionBar.SetTitle(Resource.String.openDrawer);
				}
				else
				{
					SupportActionBar.SetTitle(Resource.String.closeDrawer);
				}
			}
			else
			{
				SupportActionBar.SetTitle(Resource.String.closeDrawer);
			}
			mLeftDrawer.ItemClick += List_ItemClick;// clc del elemento menu left
			lwElem.ItemClick += ListCar_ItemClick;//click del elemento producto

			mRightDrawer.ItemClick += ListCarDelete_ItemClick;//clic para eliminar elementos del carrito
		}
		protected void ListCarDelete_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			double precio = 0;
			new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("Alerta").SetMessage("¿Deseas remover este elemento del carrito?").SetPositiveButton("ok", delegate
			 {
				 var lisView = sender as ListView;
				 var l = car[e.Position];
				 mRightDrawer.Adapter = new adapter_carrito(this, car);
				 car.Remove(car[e.Position]);
				 if (car.Count == 0)
				 {
					 precio = 0;
				 }
				 foreach (var ite in car)
				 {
					 precio += ite.Precio;
				 }
				 lbl_sub_total.Text = "Sub-Total: $ " + precio;
			 }).SetNegativeButton("Cancelar", delegate
			 {

			 }).Show();
		}
		/*Agregar al carrito*/
		protected void ListCar_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("Alerta").SetMessage("¿Deseas agregar este elemento al carrito?").SetPositiveButton("ok", delegate
			{
				var listView = sender as ListView;
				var l = elem[e.Position];
				mRightDrawer.Adapter = new adapter_carrito(this, car);
				car.Add(new cls_datos(l.Id_element, l.Name, l.Precio, l.ruta));
				double precio = 0;
				foreach (var ite in car)
				{
					precio += ite.Precio;
					lbl_sub_total.Text = "Sub-Total: $ " + precio;
				}

				Toast.MakeText(this, "Elemento agregado", ToastLength.Short).Show();
			}).SetNegativeButton("Cancelar", delegate
			{
		//Toast.MakeText(this, "Se preciono Cancelar", ToastLength.Short).Show();
	}).Show();
		}
		//clics a los elementos del menu del lado izquierdo
		protected void List_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var info = mLeftItems[e.Position];
			if (info == "Perfil")
			{
				Toast.MakeText(this, info, ToastLength.Short).Show();
				var perfi = new Intent(this, typeof(Profile));
				StartActivity(perfi);
			}
			if (info == "Salir")
			{
				var item = new Intent(this, typeof(MainActivity));
				StartActivity(item);
			}
			if (info == "Acerca de")
			{
				var item = new Intent(this, typeof(Acerca_d));
				StartActivity(item);
			}
		}
		//----------------menu de tres puntos-----------------------------------------------
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_main, menu);
			return base.OnCreateOptionsMenu(menu);
		}
		//----------------------------------------------------------------------------------

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (mDrawerToggle.OnOptionsItemSelected(item))
			{
				if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
				{
					mDrawerLayout.CloseDrawer(mRightDrawer);
				}
				return true;
			}
			switch (item.ItemId)
			{
				case Resource.Id.action_help:
					if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
					{
						mDrawerLayout.CloseDrawer(mRightDrawer);
					}
					else
					{
						mDrawerLayout.CloseDrawer(mLeftDrawer);
						mDrawerLayout.OpenDrawer(mRightDrawer);
					}
					return true;
				case Resource.Id.action_buy:
					if (car.Count == 0)
					{
						Double precio = 0;
						lbl_sub_total.Text = "Sub-Total: $ " + precio;
						Toast.MakeText(this, "El carrito se encuentra vacio no se ha realizado la accion", ToastLength.Short).Show();
					}
					else
					{
						new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("Alerta").SetMessage("¿Estas seguro de realizar esta compra?").SetPositiveButton("ok", delegate
						{
							double precio = 0;
							foreach (var ite in car)
							{
								precio += ite.Precio;
								lbl_sub_total.Text = "Sub-Total: $ " + precio;
							}
							Toast.MakeText(this, "El costo de la compra es: " + precio.ToString(), ToastLength.Short).Show();
							verDatos();
							if (saldo >= precio)
							{
								Toast.MakeText(this, "Se ha realizado la compra", ToastLength.Short).Show();
								Use us = new Use();
                                Servicio.Servicio sw = new Servicio.Servicio();
								XmlSerializer seriarlizar = new XmlSerializer(typeof(Use));
								string ruta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
								StreamReader Lectura = new StreamReader(ruta + "usuario.xml");
								Use b = (Use)seriarlizar.Deserialize(Lectura);
								Lectura.Close();
								venta();
							}
							else
							{
								precio = 0;
								Toast.MakeText(this, "No cuentas con el saldo suficiente para realizar la compra", ToastLength.Short).Show();
							}
							mRightDrawer.Adapter = new adapter_carrito(this, car);
							car.Clear();
							mRightDrawer.Adapter = new adapter_carrito(this, car);
						}).SetNegativeButton("Cancelar", delegate
						{
							Toast.MakeText(this, "Se ha cancelado la compra", ToastLength.Short).Show();
							mRightDrawer.Adapter = new adapter_carrito(this, car);
							car.Clear();
							mRightDrawer.Adapter = new adapter_carrito(this, car);
						}).Show();
					}
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}

		}
		//-------------------Lennar la lista--------------------
		public void llenarList()
		{
			lwElem = FindViewById<ListView>(Resource.Id.lwElement);
			lwElem.Adapter = new adapter_datos(this, elem);

			try
			{
                Servicio.Servicio sw = new Servicio.Servicio();
				sw.ProductoShowCompleted += Sw_ProductoShowCompleted;
				sw.ProductoShowAsync();
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
			}
		}
		void Sw_ProductoShowCompleted(object sender, Servicio.ProductoShowCompletedEventArgs e)
		{
			int id;
			string nombre;
			double precio;
			string url;
			lwElem = FindViewById<ListView>(Resource.Id.lwElement);
			lwElem.Adapter = new adapter_datos(this, elem);
			DataSet conjunto = new DataSet();
			//DataRow renglon;
			conjunto = e.Result;
			foreach (DataRow item in conjunto.Tables["comidas"].Rows)
			{
				id = int.Parse(item["IdProducto"].ToString());
				nombre = item["Nombre"].ToString();
				precio = double.Parse(item["precio"].ToString());
				url = item["url"].ToString();
				elem.Add(new cls_datos(id, nombre, precio, url));
			}
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
		void Sw_InfoUsuarioCompleted(object sender, Servicio.InfoUsuarioCompletedEventArgs e)
		{
			DataSet conjunto = new DataSet();
			DataRow renglon;
			try
			{
				conjunto = e.Result;
				renglon = conjunto.Tables["comidas"].Rows[0];
				if ((renglon["Saldo"]).ToString() == "")
				{
					saldo = 0;
					lbl_credito.Text = "Saldo disponible :$" + saldo.ToString();
				}
				else
				{
					saldo = double.Parse((renglon["Saldo"]).ToString());
					lbl_credito.Text = "Saldo disponible :$" + saldo.ToString();
				}

			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
			}
		}
		public void venta()
		{
            Servicio.Servicio sw = new Servicio.Servicio();
			Use u = new Use();
			//sw.VentaCompleted += Sw_VentaCompleted;
			//sw.VentaAsync(id);
			foreach (var ite in car)
			{
				//sw.DetalleCompleted += Sw_DetalleCompleted;
				//sw.DetalleAsync(ite.Id_element);
			}
		}

		/*void Sw_VentaCompleted(object sender, Servicio.VentaCompletedEventArgs e)
		{
			if (e.Result == true)
			{

			}
			else {
				Toast.MakeText(this, "Error", ToastLength.Short).Show();
			}
		}

		void Sw_DetalleCompleted(object sender, Servicio.DetalleCompletedEventArgs e)
		{
			if (e.Result == true)
			{

			}
			else {
				Toast.MakeText(this, "Error", ToastLength.Short).Show();
			}
		}*/

		void Sw_ActualizarDaldoCompleted(object sender, Servicio.ActualizarDaldoCompletedEventArgs e)
		{

		}

		void Inicio_Sw_InfoUsuarioCompleted(object arg1, Servicio.InfoUsuarioCompletedEventArgs arg2)
		{
		}
	}
}
