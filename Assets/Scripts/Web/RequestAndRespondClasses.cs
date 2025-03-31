using System;
using System.Collections.Generic;
using UnityEngine;

#region Login
[SerializeField]
public class PostLoginRequest
{
    public string email;
    public string password;
}

[Serializable]
public class LoginPostResponse
{
    public string token;
    public int company_id;
    public string username;
    public string company_name;
}
#endregion

#region Categories
[Serializable]
public class MaterialsData
{
    public CategoriesDataResponse data;
}

[Serializable]
public class CategoriesDataResponse
{
    public MaterialCategory[] categories;
}

[Serializable]
public class MaterialCategory
{
    public MaterialCategory[] categories;
    public int company_id;
    public int id;
    public ListedMaterial[] materials;
    public string name;
    public bool custom;
}

[Serializable]
public class ListedMaterial
{
    public int id;
    public string name;
    public bool custom = true;
    public MaterialOption[] options;
}

[Serializable]
public class MaterialOption
{
    public int id;
    public string name;
    public bool custom = true;
}
#endregion

#region Create Category
[Serializable]
public class CreateCategoryRequest
{
    public string token;
    public string name;
    public int? parent_id;
}
[Serializable]
public class CreateCategoryRespond
{
    public IDRespond data;
}
#endregion

#region Create Material
[Serializable]
public class CreateMaterialRequest
{
    public string token;
    public string name;
    public int? category_id;
}

[Serializable]
public class CreateMaterialRespond
{
    public IDRespond data;
}
#endregion

#region Create Option
[Serializable]
public class CreateOptionRequest
{
    public string token;
    public string name;
    public int material_id;
}

[Serializable]
public class CreateOptionRespond
{
    public IDRespond data;
}
#endregion

#region Delete Category
[Serializable]
public class DeleteCategoryRequest
{
    public string token;
    public int id;
    public bool detach = false;
}
#endregion

#region Delete Material
[Serializable]
public class DeleteMaterialRequest
{
    public string token;
    public int id;
}

#endregion

#region Delete Option
[Serializable]
public class DeleteOptionRequest
{
    public string token;
    public int id;
}

#endregion

[Serializable]
public class IDRespond
{
    public int id;
}

#region Create

[Serializable]
public class CreateProductRequest
{
    public string token;
    public int amount;
    public ProductData data;
}

[Serializable]
public class ProductData
{
    public int serial_number;
    public string size;
    public int qty;
    public int material_id;
    public int? slot_id;
    public int? destination_partner_id;
}

[Serializable]
public class CreateProductRespond
{
    public CreatedProduct[] data;
}

[Serializable]
public class CreatedProduct
{
    public int id;
    public string token;
}

#endregion

#region Get QR Code

[Serializable]
public class GetQRCode
{
    public string token;
    public int id;
    public string product_token;
}

#endregion

#region Transfer

[Serializable]
public class TransferRequest
{
    public string token;
    public int id;
    public string product_token;
}

#endregion

#region Scan

[Serializable]
public class ScanRequest
{
    public string token;
    public int id;
    public string product_token;
}

[Serializable]
public class ScanRespond
{
    public ScanedProductData data;
}

[Serializable]
public class ScanedProductData
{
    public ScanedProduct product;
}

[Serializable]
public class ScanedProduct
{
    public int id;
    public int serial_number;
    public string size;
    public int qty;
    public string token;
    public bool transfered;
    public int material_id;
    public int company_id;
    public string slot_id;
    public int? destination_partner_id;
}

#endregion

#region Change destination

[Serializable]
public class ChangeDestinationRequest
{
    public string token;
    public int id;
    public string product_token;
    public int? destination_company_id;
}

#endregion

#region Get Partners

[Serializable]
public class GetPartnersRequest
{
    public string token;
}

[Serializable]
public class GetPartnersRespond
{
    public PartnersData data;
}

[Serializable]
public class PartnersData
{
    public Partner[] partners;
}

[Serializable]
public class Partner
{
    public string address;
    public string company_name;
    public string contact_person;
    public string date_created;
    public string date_updated;
    public string email;
    public int id;
    public string name;
    public string phone;
}

#endregion

#region Warehouses

[Serializable]
public class GetWarehousesRespond
{
    public WarehousesData data;
}

[Serializable]
public class WarehousesData
{
    public Warehouse[] warehouses;
}

[Serializable]
public class Warehouse
{
    public int id;
    public string name;
    public int lanes_count;
}

#endregion

#region Lane

[Serializable]
public class GetLanesRespond
{
    public LanesData data;
}

[Serializable]
public class LanesData
{
    public Lane[] lanes;
}

[Serializable]
public class Lane
{
    public int id;
    public string name;
    public int racks_count;
    public Rack[] racks;
}

#endregion

#region Slot



[Serializable]
public class GetSlotRespond
{
    public SlotData data;
}

[Serializable]
public class SlotData
{
    public SlotInfo slot;
}

[Serializable]
public class SlotInfo
{
    public int id;
    public string name;
    public int slots_count;
}

#endregion

#region Racks

[Serializable]
public class Rack
{
    public int id;
    public string name;
    public int number;
    public int shelfs_count;
}

#endregion