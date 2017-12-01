package md53e6d61ab04cf8b762c100cd840ce0376;


public class CircleTransform
	extends com.bumptech.glide.load.resource.bitmap.BitmapTransformation
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getId:()Ljava/lang/String;:GetGetIdHandler\n" +
			"n_transform:(Lcom/bumptech/glide/load/engine/bitmap_recycle/BitmapPool;Landroid/graphics/Bitmap;II)Landroid/graphics/Bitmap;:GetTransform_Lcom_bumptech_glide_load_engine_bitmap_recycle_BitmapPool_Landroid_graphics_Bitmap_IIHandler\n" +
			"";
		mono.android.Runtime.register ("AppService.CircleTransform, AppService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CircleTransform.class, __md_methods);
	}


	public CircleTransform (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == CircleTransform.class)
			mono.android.TypeManager.Activate ("AppService.CircleTransform, AppService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public CircleTransform (com.bumptech.glide.load.engine.bitmap_recycle.BitmapPool p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == CircleTransform.class)
			mono.android.TypeManager.Activate ("AppService.CircleTransform, AppService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Com.Bumptech.Glide.Load.Engine.Bitmap_recycle.IBitmapPool, GlideAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public java.lang.String getId ()
	{
		return n_getId ();
	}

	private native java.lang.String n_getId ();


	public android.graphics.Bitmap transform (com.bumptech.glide.load.engine.bitmap_recycle.BitmapPool p0, android.graphics.Bitmap p1, int p2, int p3)
	{
		return n_transform (p0, p1, p2, p3);
	}

	private native android.graphics.Bitmap n_transform (com.bumptech.glide.load.engine.bitmap_recycle.BitmapPool p0, android.graphics.Bitmap p1, int p2, int p3);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
