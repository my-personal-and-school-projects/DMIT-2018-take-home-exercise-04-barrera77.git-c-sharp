﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

[Index("ProductId", Name = "IDX_VendorCatalogs_ProductID")]
[Index("VendorId", Name = "IDX_VendorCatalogs_VendorID")]
public partial class VendorCatalog
{
    [Key]
    [Column("VendorCatalogID")]
    public int VendorCatalogId { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column("VendorID")]
    public int VendorId { get; set; }

    [Required]
    [StringLength(6)]
    [Unicode(false)]
    public string OrderUnitType { get; set; }

    public int OrderUnitSize { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderUnitCost { get; set; }

    public bool RemoveFromViewFlag { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("VendorCatalogs")]
    public virtual Product Product { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("VendorCatalogs")]
    public virtual Vendor Vendor { get; set; }
}