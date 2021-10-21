import { ICustomer } from "@/types/Customer";
import { IServiceResponse } from "@/types/ServiceResponse";
import axios from "axios";

/**
 * Customer Service
 * Provides UI business logic associated with Customers in our system
 */
export default class CustomerService {
    API_URL = process.env.VUE_APP_API_URL;

    public async getCustomers(): Promise<ICustomer[]> {
        const result: any = await axios.get(`${this.API_URL}/customer/`);
        return result.data;
    }

    public async addCustomers(newCustomer: ICustomer): Promise<IServiceResponse<ICustomer>> {
        const result: any = await axios.post(`${this.API_URL}/customer/`, newCustomer);
        return result.data;
    }

    public async deleteCustomers(customerId: number): Promise<boolean> {
        const result: any = await axios.delete(`${this.API_URL}/customer/${customerId}`);
        return result.data;
    }
}