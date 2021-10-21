<template>
  <div>
    <h1 id="invoiceTitle">
      Create Invoice
    </h1>
    <hr/>
    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select v-model="selectedCustomerId" class="invoiceCustomers" id="customer">
          <option disabled value="">Please select a Customer</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">{{ c.firstName + " " + c.lastName }}</option>
        </select>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>

      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select v-model="newItem.product" class="invoiceLineItem" id="product">
          <option disabled value="">Please select a Product</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input v-model="newItem.quantity" id="quantity" type="number" min="0" />
      </div>

      <div class="invoice-item-actions">
        <solar-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
        >
          Add Line Item
        </solar-button>

        <solar-button
          :disabled="!lineItems.length"
          @button:click="finalizeOrder"
        >
          Finalize Order
        </solar-button>
      </div>
    </div>
    <div class="invoice-step" v-if="invoiceStep === 3">

    </div>
    <hr/>

    <div class="invoice-steps-actions">
      <solar-button @button:click="prev" :disabled="!canGoPrev">Previous</solar-button>
      <solar-button @button:click="next" :disabled="!canGoNext">Next</solar-button>
      <solar-button @button:click="startOver">Previous</solar-button>
    </div>
  </div>
</template>

<script lang="ts">
   import { Component, Vue } from 'vue-property-decorator';
   import {IInvoice, ILineItem} from '@/types/IInvoice';
   import {ICustomer} from '@/types/Customer';
   import {IProductInventory} from '@/types/Product';
   import CustomerService from '@/services/customer-service';
   import {InventoryService} from '@/services/inventory-service';
   import InvoiceService from '@/services/invoice-service';
   import SolarButton from '@/components/SolarButton.vue';

   const customerService = new CustomerService();
   const inventoryService = new InventoryService();
   const invoiceService = new InvoiceService();

   @Component({ name: "Create Invoice", components: { SolarButton } })

   export default class CreateInvoice extends Vue {
       invoiceStep = 1;
       invoice: IInvoice = {
        createdOn: new Date(),
        updatedOn: new Date(),
        customerId: 0,
        lineItems: []
       };
       customers: ICustomer[] = [];
       selectedCustomerId = 0;
       inventory: IProductInventory[] = [];
       lineItems: ILineItem[] = [];
       newItem: ILineItem = { 
         product: undefined,
         quantity: 0
       };

       addLineItem() {
         const newItem: ILineItem = {
           product: this.newItem.product,
           //quantity: parseInt(this.newItem.quantity.toString()),
           quantity: Number(this.newItem.quantity)
         };

         const existingItems = this.lineItems.map(item => item.product.id);

         if (existingItems.includes(newItem.product.id)) {
           const lineItem = this.lineItems.find(
             item => item.product.id === newItem.product.id
           );

           let currentQuantity = Number(lineItem.quantity);
           const updatedQuantity = currentQuantity += newItem.quantity;
           lineItem.quantity = updatedQuantity;
         } else {
           this.lineItems.push(this.newItem);
         }

         this.newItem = { product: undefined, quantity: 0 };
       }

       startOver(): void {
         this.invoice = { customerId: 0, lineItems: [] };
         this.invoiceStep = 1;
       }

       finalizeOrder() {
         this.invoiceStep = 3;
       }

       get canGoNext() {
         if (this.invoiceStep === 1) {
           return this.selectedCustomerId !== 0;
         }

         if (this.invoiceStep === 2) {
           return true;
         }
         
         if (this.invoiceStep === 3) {
           return false;
         }

         return false;
       }

       canGoPrev() {
         return this.invoiceStep != 1
       }

       prev(): void {
         if (this.invoiceStep === 1) {
           return;
         }
         this.invoiceStep -= 1;
       }

       next(): void {
         if (this.invoiceStep === 3) {
           return;
         }
         this.invoiceStep += 1;
       }

      async initialize(): Promise<void> {
        this.customers = await customerService.getCustomers();
        this.inventory = await inventoryService.getInventory();
      }

      async created() {
        await this.initialize();
      }
   }
</script>

<style scoped lang="scss">
.invoice-step {

}
.invoice-step-detail {
  margin: 1.2rem;
}
    

</style>