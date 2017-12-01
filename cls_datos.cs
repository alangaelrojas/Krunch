using System;
namespace AppService
{
	public class cls_datos
	{
		int _id_element;
		string _name;
		double _precio;
		string _ruta;
		public cls_datos(int id_element, string name, double precio, string ruta)
		{
			this._id_element = id_element;
			this._name = name;
			this._precio = precio;
			this._ruta = ruta;
		}
		public int Id_element
		{
			get
			{
				return _id_element;
			}
			set
			{
				_id_element = value;
			}
		}
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		public double Precio
		{
			get
			{
				return _precio;
			}
			set
			{
				_precio = value;
			}
		}
		public string ruta
		{
			get
			{
				return _ruta;
			}
			set
			{
				_ruta = value;
			}
		}	}
}