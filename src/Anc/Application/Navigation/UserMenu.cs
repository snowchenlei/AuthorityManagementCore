﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Application.Navigation
{
    public class UserMenu
    {
        /// <summary>
        /// Unique name of the menu in the application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the menu.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A custom object related to this menu item.
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Menu items (first level).
        /// </summary>
        public IList<UserMenuItem> Items { get; set; }

        public UserMenu AddItem(UserMenuItem menuItem)
        {
            Items.Add(menuItem);
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="UserMenu"/> object.
        /// </summary>
        public UserMenu()
        {
        }

        /// <summary>
        /// Creates a new <see cref="UserMenu"/> object from given <see cref="MenuDefinition"/>.
        /// </summary>
        public UserMenu(MenuDefinition menuDefinition)
        {
            Name = menuDefinition.Name;
            DisplayName = menuDefinition.DisplayName;
            CustomData = menuDefinition.CustomData;
            Items = new List<UserMenuItem>();
        }
    }
}