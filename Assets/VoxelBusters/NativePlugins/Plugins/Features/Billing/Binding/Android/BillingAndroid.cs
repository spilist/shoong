using UnityEngine;
using System.Collections;

#if USES_BILLING && UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class BillingAndroid : Billing 
	{

		#region Constructors

		BillingAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
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

			string[] _consumableProductIDs, _nonConsumableProductIDs;
			
			// Get consumable and non consumable product ids
			ExtractProductIDs(_settings.Products, out _consumableProductIDs, 
			                  out _nonConsumableProductIDs);

			// Native store init is called
			Plugin.Call(Native.Methods.INITIALIZE,_publicKey, _consumableProductIDs.ToJSON()); //Update with consumable products initially. 

		}
		protected override void RequestForBillingProducts (string[] _consumableProductIDs, string[] _nonConsumableProductIDs)			
		{
			// Send request to native store
			Plugin.Call(Native.Methods.REQUEST_BILLING_PRODUCTS,_consumableProductIDs.ToJSON(), _nonConsumableProductIDs.ToJSON());
		}
		
		public override bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= Plugin.Call<bool>(Native.Methods.IS_PRODUCT_PURCHASED,_productID);
			Console.Log(Constants.kDebugTag, "[Billing] Product=" + _productID + " IsPurchased=" + _isPurchased);
			
			return _isPurchased;
		}
		
		public override void BuyProduct (string _productID)
		{
			if(!string.IsNullOrEmpty(_productID))
			{
				Plugin.Call(Native.Methods.BUY_PRODUCT,_productID);
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
			Plugin.Call(Native.Methods.RESTORE_COMPLETED_TRANSACTIONS);
		}
		
		#endregion
	}
}
#endif