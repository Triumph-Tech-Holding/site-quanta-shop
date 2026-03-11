import { defineStore } from 'pinia';
import { ref } from 'vue';
import { contact } from "@/services/contact-service";

export const useContactStore = defineStore('conctac-store', () => {    
    async function conctact(conctactData: any) {        

        try {            
            const response = await contact(conctactData);                        
        } catch (error) {
            console.error('Erro ao realizar o contato:', error);
        }
    }

    return { conctact }
});