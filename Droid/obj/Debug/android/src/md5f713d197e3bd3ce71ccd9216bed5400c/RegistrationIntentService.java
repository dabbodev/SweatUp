package md5f713d197e3bd3ce71ccd9216bed5400c;


public class RegistrationIntentService
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("SweatUp.Droid.RegistrationIntentService, SweatUp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RegistrationIntentService.class, __md_methods);
	}


	public RegistrationIntentService (java.lang.String p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == RegistrationIntentService.class)
			mono.android.TypeManager.Activate ("SweatUp.Droid.RegistrationIntentService, SweatUp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public RegistrationIntentService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RegistrationIntentService.class)
			mono.android.TypeManager.Activate ("SweatUp.Droid.RegistrationIntentService, SweatUp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

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