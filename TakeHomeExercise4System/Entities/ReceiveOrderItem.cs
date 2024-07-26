﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

[Index("OrderDetailId", Name = "IDX_ReceiveOrderItems_OrderDetailID")]
[Index("ReceiveOrderId", Name = "IDX_ReceiveOrderItems_ReceiveOrderID")]
public partial class ReceiveOrderItem
{
    [Key]
    [Column("ReceiveOrderItemID")]
    public int ReceiveOrderItemId { get; set; }

    [Column("ReceiveOrderID")]
    public int ReceiveOrderId { get; set; }

    [Column("OrderDetailID")]
    public int OrderDetailId { get; set; }

    public int ItemQuantity { get; set; }

    public bool RemoveFromViewFlag { get; set; }

    [ForeignKey("OrderDetailId")]
    [InverseProperty("ReceiveOrderItems")]
    public virtual OrderDetail OrderDetail { get; set; }

    [ForeignKey("ReceiveOrderId")]
    [InverseProperty("ReceiveOrderItems")]
    public virtual ReceiveOrder ReceiveOrder { get; set; }
}