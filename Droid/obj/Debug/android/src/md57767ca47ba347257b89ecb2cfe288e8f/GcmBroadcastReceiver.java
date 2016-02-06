package md57767ca47ba347257b89ecb2cfe288e8f;


public class GcmBroadcastReceiver
	extends md5214eafb7e7b3b7fcc363a68a6358563f.GcmBroadcastReceiverBase_1
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("sweatup.Droid.GcmBroadcastReceiver, SweatUp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GcmBroadcastReceiver.class, __md_methods);
	}


	public GcmBroadcastReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GcmBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("sweatup.Droid.GcmBroadcastReceiver, SweatUp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
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
