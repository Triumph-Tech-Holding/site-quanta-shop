<template>
  <form id="contact-form" @submit="onSubmit">
      <div class="tp-contact-input-wrapper">
        <div class="tp-contact-input-box">
            <div class="tp-contact-input">
              <input name="name" id="name" type="text" placeholder="Seu nome aqui" v-bind="name">
            </div>
            <div class="tp-contact-input-title">
              <label for="name">Nome</label>
            </div>
            <err-message :msg="errors.name" />
        </div>
        <div class="tp-contact-input-box">
            <div class="tp-contact-input">
              <input name="email" id="email" type="email" placeholder="seu@mail.com" v-bind="email">
            </div>
            <div class="tp-contact-input-title">
              <label for="email">Email</label>
            </div>
            <err-message :msg="errors.email" />
        </div>
        <div class="tp-contact-input-box d-none">
            <div class="tp-contact-input">
              <input name="subject" id="subject" type="text" placeholder="Escreva aqui o assunto" v-bind="subject">
            </div>
            <div class="tp-contact-input-title">
              <label for="subject">Subject</label>
            </div>
            <err-message :msg="errors.subject" />
        </div>
        <div class="tp-contact-input-box">
            <div class="tp-contact-input">
              <Field name="message" v-slot="{ field }">
                <textarea v-bind="field" id="message" name="message" placeholder="Escreva sua mensagem aqui..."></textarea>
              </Field>
            </div>
            <div class="tp-contact-input-title">
              <label for="message">Mensagem</label>
            </div>
            <err-message :msg="errors.message" />
        </div>
      </div>
      <div class="tp-contact-suggetions mb-20 d-none">
        <div class="tp-contact-remeber">
            <input id="remeber" type="checkbox">
            <label for="remeber">Save my name, email, and website in this browser for the next time I comment.</label>
        </div>
      </div>
      <div class="tp-contact-btn">
        <button type="submit">Enviar</button>
      </div>
  </form>
</template>

<script setup lang="ts"> 
import { useForm,Field } from 'vee-validate';
import * as yup from 'yup';
import { useContactStore } from "@/pinia/useContactStore";


const contactStore = useContactStore();
const { conctact } = contactStore; 

interface IFormValues {
  name?: string | null;
  email?: string | null;
  subject?: string | null;
  message?: string | null;
}
const { errors, handleSubmit, defineInputBinds,resetForm } = useForm<IFormValues>({
  validationSchema: yup.object({
    name: yup.string().required("O campo Nome é obrigatório").label("Name"),
    email: yup.string().required("O campo Email é obrigatório").email("O email deve ser um email válido").label("Email"),    
    message: yup.string().required("O campo mensagem é obrigatório").label("Message")
  }),
});

const onSubmit = handleSubmit(async values => {
  try {  
    await conctact({
      nome: values.name,
      email: values.email,
      mensagem: values.message
    });    
    // resetForm();
  } catch (error) {
    console.error("Erro ao realizar o contato login:", error);
  }
  // resetForm()
});

const name = defineInputBinds('name');
const email = defineInputBinds('email');
const subject = defineInputBinds('subject');
</script>
