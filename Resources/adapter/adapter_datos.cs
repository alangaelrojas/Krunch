using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.AppCompat;
using Android.Support.V7.Widget;
using System.Net;
using Java.Net;
using Java.IO;
using System.IO;
using Android.Graphics;

namespace AppService
{
	public class adapter_datos : BaseAdapter<cls_datos>
	{
		List<cls_datos> items;
		Activity context;
		public adapter_datos(Activity context, List<cls_datos> items) : base()
		{
			this.context = context;
			this.items = items;
		}
		public override cls_datos this[int position]
		{
			get
			{
				return items[position];
			}
		}

		public override int Count
		{
			get
			{
				return items.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.items_def, null);

			view.FindViewById<TextView>(Resource.Id.txt_name).Text = item.Name;
			view.FindViewById<TextView>(Resource.Id.txt_price).Text = "$" + item.Precio.ToString();

			//int id_img = context.Resources.GetIdentifier(item.ruta, "drawable", context.PackageName);
			//view.FindViewById<ImageView>(Resource.Id.iv_avatar).SetImageResource(id_img);


			//view.FindViewById<ImageView>(Resource.Id.iv_avatar).SetImageBitmap(GetImageBitmapFromUrl(item.ruta));

			return view;
		}
		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}	}
}
