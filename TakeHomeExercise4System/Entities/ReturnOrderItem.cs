﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

[Index("OrderDetailId", Name = "IDX_ReturnOrderItems_OrderDetailID")]
[Index("ReceiveOrderId", Name = "IDX_ReturnOrderItems_ReceiveOrderID")]
public partial class ReturnOrderItem
{
    [Key]
    [Column("ReturnOrderItemID")]
    public int ReturnOrderItemId { get; set; }

    [Column("ReceiveOrderID")]
    public int ReceiveOrderId { get; set; }

    [Column("OrderDetailID")]
    public int? OrderDetailId { get; set; }

    [StringLength(50)]
    public string UnOrderedItem { get; set; }

    public int ItemQuantity { get; set; }

    [StringLength(100)]
    public string Comment { get; set; }

    [Column("VendorProductID")]
    [StringLength(25)]
    public string VendorProductId { get; set; }

    public bool RemoveFromViewFlag { get; set; }

    [ForeignKey("OrderDetailId")]
    [InverseProperty("ReturnOrderItems")]
    public virtual OrderDetail OrderDetail { get; set; }

    [ForeignKey("ReceiveOrderId")]
    [InverseProperty("ReturnOrderItems")]
    public virtual ReceiveOrder ReceiveOrder { get; set; }
}