using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.AppCompat;
using Android.Support.V7.Widget;

namespace AppService
{
	public class adapter_carrito : BaseAdapter<cls_datos>
	{
		List<cls_datos> items;
		Activity context;
		public adapter_carrito(Activity context, List<cls_datos> items) : base()
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
				view = context.LayoutInflater.Inflate(Resource.Layout.item_carrito, null);

			view.FindViewById<TextView>(Resource.Id.txt_name).Text = item.Name;
			view.FindViewById<TextView>(Resource.Id.txt_price).Text = "$" + item.Precio.ToString();

			return view;

		}
	}
}