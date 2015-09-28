using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class BillingAndroid : Billing 
	{
		#region Platform Native Info

		class NativeInfo
		{
			// Handler class name
			public class Class
			{
				public const string NAME								= "com.voxelbusters.nativeplugins.features.billing.BillingHandler";
			}
	
			// For holding method names
			public class Methods
			{
				public const string INITIALIZE		 					= "initialize";
				public const string REQUEST_BILLING_PRODUCTS 			= "requestBillingProducts"; 
				public const string IS_PRODUCT_PURCHASED				= "isProductPurchased";
				public const string BUY_PRODUCT							= "buyProduct";
				public const string CUSTOM_VERIFICATION_FINISHED		= "customVerificationFinished";
				public const string RESTORE_COMPLETED_TRANSACTIONS 		= "restoreCompletedTransactions";
			}
		}

		#endregion
	
		#region  Required Variables

		private AndroidJavaObject 	m_plugin;
		private AndroidJavaObject  	Plugin
		{
			get 
			{ 
				if(m_plugin == null)
				{
					Console.LogError(Constants.kDebugTag, "[Billing] Plugin class not intialized!");
				}
				return m_plugin; 
			}

			set
			{
				m_plugin = value;
			}
		}
		
		#endregion

		#region Constructors

		BillingAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}

		#endregion

		#region Overriden API's
		
		protected override void Initialise (BillingSettings _settings)
		{
			string _publicKey	= _settings.Android.PublicKey;

			if(string.IsNullOrEmpty(_publicKey))
			{
				Console.LogError(Constants.kDebugTag, "[Billing] Please specify public key in the configuration to proceed");
				return;
			}

			List<string> _consumableProductIDs, _nonConsumableProductIDs;
			
			// Get consumable and non consumable product ids
			ExtractProductIDs(_settings.Products, out _consumableProductIDs, 
			                  out _nonConsumableProductIDs);

			// Native store init is called
			Plugin.Call(NativeInfo.Methods.INITIALIZE,_publicKey, _consumableProductIDs.ToJSON()); //Update with consumable products initially. 

		}
		
		protected override void RequestForBillingProducts (List<string> _consumableProductIDs, List<string> _nonConsumableProductIDs)
		{
			// Send request to native store
			Plugin.Call(NativeInfo.Methods.REQUEST_BILLING_PRODUCTS,_consumableProductIDs.ToJSON(), _nonConsumableProductIDs.ToJSON());
		}
		
		public override bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= Plugin.Call<bool>(NativeInfo.Methods.IS_PRODUCT_PURCHASED,_productID);
			Console.Log(Constants.kDebugTag, "[Billing] Product=" + _productID + " IsPurchased=" + _isPurchased);
			
			return _isPurchased;
		}
		
		public override void BuyProduct (string _productID)
		{
			if(!string.IsNullOrEmpty(_productID))
			{
				Plugin.Call(NativeInfo.Methods.BUY_PRODUCT,_productID);
			}
			else
			{
				Console.LogError(Constants.kDebugTag, "[Billing]_productID is null!");
			}
		}		
		
		public override void CustomVerificationFinished (BillingTransaction _transaction)
		{
			//Nothing to do here. Not supporting external validation for android. //TODO - This needs original payload to verify
			Console.Log(Constants.kDebugTag, "[Billing] Android has done its validation already internally after purchase. so this call have no effect on Android");
		}

		public override void RestoreCompletedTransactions ()
		{
			Plugin.Call(NativeInfo.Methods.RESTORE_COMPLETED_TRANSACTIONS);
		}
		
		#endregion
	}
}
#endif