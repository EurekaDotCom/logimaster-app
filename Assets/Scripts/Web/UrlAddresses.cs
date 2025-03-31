using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UrlAddresses
{
    private const string Address_dev = "http://192.168.88.13:5001";
    private const string Address_build = "http://eurekait.duckdns.org:5001";

    private static string MaterialsData_dev = $"{Address_dev}/api/materials/get_data";
    private static string MaterialsData_build = $"{Address_build}/api/materials/get_data";
    public static string MaterialsData
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return MaterialsData_dev;
#else
            return MaterialsData_build;
#endif

        }
    }
    private static string LoginRequest_dev = $"{Address_dev}/api/auth/login";
    private static string LoginRequest_build = $"{Address_build}/api/auth/login";

    public static string LoginRequest
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return LoginRequest_dev;
#else
            return LoginRequest_build;
#endif
        }
    }

    private static string LogOutRequest_dev = $"{Address_dev}/api/auth/logout";
    private static string LogOutRequest_build = $"{Address_build}/api/auth/logout";
    public static string LogOutRequest
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return LogOutRequest_dev;
#else
            return LogOutRequest_build;
#endif
        }
    }

    private static string CreateMaterialCategory_dev = $"{Address_dev}/api/materials/create_category";
    private static string CreateMaterialCategory_build = $"{Address_build}/api/materials/create_category";
    public static string CreateMaterialCategory
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return CreateMaterialCategory_dev;
#else
            return CreateMaterialCategory_build;
#endif
        }
    }

    private static string CreateMaterial_dev = $"{Address_dev}/api/materials/create_material";
    private static string CreateMaterial_build = $"{Address_build}/api/materials/create_material";
    public static string CreateMaterial
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return CreateMaterial_dev;
#else
            return CreateMaterial_build;
#endif
        }
    }

    private static string CreateOption_dev = $"{Address_dev}/api/materials/create_option";
    private static string CreateOption_build = $"{Address_build}/api/materials/create_option";
    public static string CreateOption
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return CreateOption_dev;
#else
            return CreateOption_build;
#endif
        }
    }

    private static string DeleteMaterialCategory_dev = $"{Address_dev}/api/materials/delete_category";
    private static string DeleteMaterialCategory_build = $"{Address_build}/api/materials/delete_category";
    public static string DeleteMaterialCategory
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return DeleteMaterialCategory_dev;
#else
            return DeleteMaterialCategory_build;
#endif
        }
    }

    private static string DeleteMaterial_dev = $"{Address_dev}/api/materials/delete_material";
    private static string DeleteMaterial_build = $"{Address_build}/api/materials/delete_material";
    public static string DeleteMaterial
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return DeleteMaterial_dev;
#else
            return DeleteMaterial_build;
#endif
        }
    }

    private static string DeleteOption_dev = $"{Address_dev}/api/materials/delete_option";
    private static string DeleteOption_build = $"{Address_build}/api/materials/delete_option";
    public static string DeleteOption
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return DeleteOption_dev;
#else
            return DeleteOption_build;
#endif
        }
    }

    private static string GetPartners_dev = $"{Address_dev}/api/partners/get";
    private static string GetPartners_build = $"{Address_build}/api/partners/get";
    public static string GetPartners
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetPartners_dev;
#else
            return GetPartners_build;
#endif
        }
    }

    private static string CreateProduct_dev = $"{Address_dev}/api/products/create";
    private static string CreateProduct_build = $"{Address_build}/api/products/create";
    public static string CreateProduct
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return CreateProduct_dev;
#else
            return CreateProduct_build;
#endif
        }
    }

    private static string GetWarehouses_dev = $"{Address_dev}/api/warehouse/get";
    private static string GetWarehouses_build = $"{Address_build}/api/warehouse/get";
    public static string GetWarehouses
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetWarehouses_dev;
#else
            return GetWarehouses_build;
#endif
        }
    }

    private static string GetLanes_dev = $"{Address_dev}/api/warehouse/get_lanes";
    private static string GetLanes_build = $"{Address_build}/api/warehouse/get_lanes";
    public static string GetLanes
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetLanes_dev;
#else
            return GetLanes_build;
#endif
        }
    }

    private static string GetProductFromScan_dev = $"{Address_dev}/api/products/scan";
    private static string GetProductFromScan_build = $"{Address_build}/api/products/scan";
    public static string GetProductFromScan
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetProductFromScan_dev;
#else
            return GetProductFromScan_build;
#endif
        }
    }

    private static string GetSlot_dev = $"{Address_dev}/api/warehouse/get_slot";
    private static string GetSlot_build = $"{Address_build}/api/warehouse/get_slot";
    public static string GetSlot
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetSlot_dev;
#else
            return GetSlot_build;
#endif
        }
    }

    private static string GetPrinters_dev = $"{Address_dev}/api/products/printers";
    private static string GetPrinters_build = $"{Address_build}/api/products/printers";
    public static string GetPrinters
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return GetPrinters_dev;
#else
            return GetPrinters_build;
#endif
        }
    }
    
    private static string CreatePrinterTask_dev = $"{Address_dev}/api/products/tasks";
    private static string CreatePrinterTask_build = $"{Address_build}/api/products/tasks";
    public static string CreatePrinterTask
    {
        get
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return CreatePrinterTask_dev;
#else
            return CreatePrinterTask_build;
#endif
        }
    }
}
