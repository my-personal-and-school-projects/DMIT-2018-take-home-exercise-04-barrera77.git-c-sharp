﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

public partial class UnOrderedItem
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [Required]
    [StringLength(50)]
    public string ItemName { get; set; }

    [Required]
    [Column("VendorProductID")]
    [StringLength(25)]
    public string VendorProductId { get; set; }

    public int Quantity { get; set; }

    public bool RemoveFromViewFlag { get; set; }
}