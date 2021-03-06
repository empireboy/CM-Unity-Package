﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Essentials
{
	[ExecuteInEditMode]
	public abstract class Entity : MonoBehaviour
	{
		[Header("Entity")]

		private Type[] _moduleInterfaces = new Type[0];
		public Type[] ModuleInterfaces { get => _moduleInterfaces; }

		private string _moduleParentName = "Modules";

		private void Awake()
		{
			OnAwake();
		}

		private void Start()
		{
			OnStart();
		}

		private void OnEnable()
		{
			// Activate modules
			List<Component> modules = new List<Component>();

			for (int i = 0; i < _moduleInterfaces.Length; i++)
			{
				Component[] tmpModules = GetModules(_moduleInterfaces[i], true);

				foreach (Component module in tmpModules)
				{
					// Not including duplicates
					if (!modules.Contains(module))
						modules.Add(module);
				}
			}

			foreach (Component module in modules)
			{
				module.gameObject.SetActive(true);
			}
		}

		protected virtual void OnAwake()
		{
			if (!CM_Debug.CategoryExists("CM", "CM.Entity"))
				CM_Debug.AddCategory(false, "CM", "CM.Entity");

			// Create Module Parent
			if (!GetModuleObject())
			{
				GameObject moduleParent = new GameObject();
				moduleParent.transform.parent = transform;
				moduleParent.name = _moduleParentName;
			}

			// Get Modules, check if the module parent exists
			if (GetModuleObject())
				InitializeModules();
		}

		protected virtual void OnStart()
		{
			// This can be overridden
		}

		public void InitializeModules()
		{
			CM_Debug.Log("Initializing Modules", "CM", "CM.Entity");
			_moduleInterfaces = this.GetType().GetInterfaces();
		}

		public GameObject GetModuleObject()
		{
			Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
			foreach (Transform child in children)
			{
				if (child.name == _moduleParentName)
				{
					return child.gameObject;
				}
			}

			return null;
		}

		public T[] GetModules<T>()
		{
			return GetModules<T>(false);
		}

		public T[] GetModules<T>(bool includeInactive)
		{
			CM_Debug.Log("Getting Modules of " + typeof(T).ToString(), "CM", "CM.Entity");

			GameObject moduleObject = GetModuleObject();

			List<T> modulesList = new List<T>();
			if (moduleObject)
			{
				T[] tmpModules;

				// Add components
				tmpModules = moduleObject.GetComponents<T>();
				foreach (T module in tmpModules)
				{
					// Not including duplicates
					if (!modulesList.Contains(module))
						modulesList.Add(module);
				}

				// Add components from all children
				tmpModules = moduleObject.GetComponentsInChildren<T>(includeInactive);
				foreach (T module in tmpModules)
				{
					// Not including duplicates
					if (!modulesList.Contains(module))
						modulesList.Add(module);
				}
			}

			T[] modules = modulesList.ToArray();

			return modules;
		}

		public Component[] GetModules(Type moduleInterface)
		{
			return GetModules(moduleInterface, false);
		}

		public Component[] GetModules(Type moduleInterface, bool includeInactive)
		{
			CM_Debug.Log("Getting Modules of " + moduleInterface.ToString(), "CM", "CM.Entity");

			GameObject moduleObject = GetModuleObject();

			List<Component> modulesList = new List<Component>();
			if (moduleObject)
			{
				Component[] tmpModules;

				// Add components
				tmpModules = moduleObject.GetComponents(moduleInterface);
				foreach (Component module in tmpModules)
				{
					// Not including duplicates
					if (!modulesList.Contains(module))
						modulesList.Add(module);
				}

				// Add components from all children
				tmpModules = moduleObject.GetComponentsInChildren(moduleInterface, includeInactive);
				foreach (Component module in tmpModules)
				{
					// Not including duplicates
					if (!modulesList.Contains(module))
						modulesList.Add(module);
				}
			}

			Component[] modules = modulesList.ToArray();

			return modules;
		}

		public void ActivateAllModules()
		{
			Transform[] childs = GetModuleObject().GetComponentsInChildren<Transform>(true);

			for (int i = 0; i < childs.Length; i++)
			{
				childs[i].gameObject.SetActive(true);
			}
		}

		public void DeactivateAllModules()
		{
			Transform[] childs = GetModuleObject().GetComponentsInChildren<Transform>(true);

			for (int i = 0; i < childs.Length; i++)
			{
				childs[i].gameObject.SetActive(false);
			}
		}
	}
}