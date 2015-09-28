using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public partial class BillingAndroid : Billing 
	{
		#region Parse Methods

		protected override void ParseProductData (IDictionary _productDict, out BillingProduct _product)
		{
			_product		= new AndroidBillingProduct(_productDict);
		}

		protected override void ParseTransactionData (IDictionary _transactionDict, out BillingTransaction _transaction)
		{
			_transaction	= new AndroidBillingTransaction(_transactionDict);
		}

		#endregion
	}
}
#endif