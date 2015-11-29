using UnityEngine;
using System.Collections;

#if USES_BILLING && UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public partial class BillingAndroid : Billing 
	{
		#region Constants
		
		private		const 	string		kProductsKey		= "products-list";
		private		const 	string		kTransactionsKey	= "transactions-list";
		private 	const 	string		kErrorKey			= "error";
		
		#endregion
		
		#region Callback Methods
		
		protected override void DidReceiveBillingProducts (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_error		= _dataDict.GetIfAvailable<string>(kErrorKey);
			
			if (_error != null)
			{
				DidReceiveBillingProducts(null, _error);
				return;
			}
			else
			{
				IList				_regProductsJSONList	= _dataDict.GetIfAvailable<IList>(kProductsKey);
				BillingProduct[]	_regProductsList		= null;
				
				if (_regProductsJSONList != null)
				{
					_regProductsList	= new AndroidBillingProduct[_regProductsJSONList.Count];
					int		_iter		= 0;
					
					foreach (IDictionary _productInfoDict in _regProductsJSONList)
					{
						_regProductsList[_iter++]			= new AndroidBillingProduct(_productInfoDict);
					}
				}
				
				DidReceiveBillingProducts(_regProductsList, null);
				return;
				
			}
		}
		
		protected override void DidFinishBillingTransaction (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_error		= _dataDict.GetIfAvailable<string>(kErrorKey);
			
			if (_error != null)
			{
				DidFinishBillingTransaction(null, _error);
				return;
			}
			else
			{
				IList					_transactionsJSONList	= _dataDict.GetIfAvailable<IList>(kTransactionsKey);
				BillingTransaction[] 	_transactionsList		= null;
				
				if (_transactionsJSONList != null)
				{
					_transactionsList	= new AndroidBillingTransaction[_transactionsJSONList.Count];
					int		_iter		= 0;
					
					foreach (IDictionary _transactionInfoDict in _transactionsJSONList)
					{
						_transactionsList[_iter++]	= new AndroidBillingTransaction(_transactionInfoDict);
					}
				}
				
				DidFinishBillingTransaction(_transactionsList, null);
				return;
			}
		}

		#endregion
	}
}
#endif