﻿using UnityEngine;

namespace CM.Essentials.Inventory
{
	public class ItemSlot : MonoBehaviour
	{
		public int index = 0;
		public ItemInstance itemInstance = null;
		public GameObject prefabInstance = null;
		public Transform slotTransform;

		public void SetItem(ItemInstance itemInstance)
		{
			this.itemInstance = itemInstance;
			this.prefabInstance = Instantiate(itemInstance.item.physicalRepresentation, slotTransform);
		}

		public void RemoveItem()
		{
			this.itemInstance = null;
			Destroy(this.prefabInstance);
			this.prefabInstance = null;
		}
	}
}