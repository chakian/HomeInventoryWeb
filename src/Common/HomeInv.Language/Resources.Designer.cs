﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeInv.Language {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HomeInv.Language.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ev seçimi olmadan kategori kaydedilemez!.
        /// </summary>
        public static string Category_HomeIsMandatory {
            get {
                return ResourceManager.GetString("Category_HomeIsMandatory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aynı isimde başka bir kategori mevcut.
        /// </summary>
        public static string Category_SameNameExists {
            get {
                return ResourceManager.GetString("Category_SameNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ev seçimi zorunludur..
        /// </summary>
        public static string HomeSelectionIsMandatory {
            get {
                return ResourceManager.GetString("HomeSelectionIsMandatory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bu evde, bu isimde bir nesne zaten var!.
        /// </summary>
        public static string ItemNameExists {
            get {
                return ResourceManager.GetString("ItemNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Kategori oluşturuldu..
        /// </summary>
        public static string Success_Category_Create {
            get {
                return ResourceManager.GetString("Success_Category_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to İşlem başarıyla gerçekleşti..
        /// </summary>
        public static string Success_Generic {
            get {
                return ResourceManager.GetString("Success_Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yeni ev oluşturuldu..
        /// </summary>
        public static string Success_Home_Create {
            get {
                return ResourceManager.GetString("Success_Home_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ev başarıyla güncellendi..
        /// </summary>
        public static string Success_Home_Edit {
            get {
                return ResourceManager.GetString("Success_Home_Edit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bu kullanıcının mevcut bir evi bulunuyor!.
        /// </summary>
        public static string UserAlreadyHasAHome {
            get {
                return ResourceManager.GetString("UserAlreadyHasAHome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bu kullanıcı zaten bu evde bulunuyor!.
        /// </summary>
        public static string UserIsAlreadyInThatHome {
            get {
                return ResourceManager.GetString("UserIsAlreadyInThatHome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uygulamayı kullanmaya başlamadan önce ilk evinizi oluşturmanız gerekiyor..
        /// </summary>
        public static string Warning_HomeNeededToUseTheApp {
            get {
                return ResourceManager.GetString("Warning_HomeNeededToUseTheApp", resourceCulture);
            }
        }
    }
}
