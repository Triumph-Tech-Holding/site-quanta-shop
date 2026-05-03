<template>
  <form @submit.prevent="handleSubmit" class="bg-white rounded pb_form_v1" novalidate>
    <h2 class="mb-4 mt-0 text-center">Informe seus dados</h2>

    <div class="form-group d-flex align-items-center">
      <input
        v-mask="'###.###.###-##'"
        v-model="form.cpf"
        type="text"
        :class="['form-control pb_height-50 reverse', errors.cpf ? 'border border-danger rounded' : '']"
        placeholder="CPF"
        autocomplete="off"
      />
      <span v-if="errors.cpf" class="d-inline-block ml-1" :title="errors.cpf">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group d-flex align-items-center">
      <input
        v-mask="'(##) #####-####'"
        v-model="form.whatsapp"
        type="text"
        :class="['form-control pb_height-50 reverse', errors.whatsapp ? 'border border-danger rounded' : '']"
        placeholder="WhatsApp"
        autocomplete="off"
      />
      <span v-if="errors.whatsapp" class="d-inline-block ml-1" :title="errors.whatsapp">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group d-flex align-items-center">
      <input
        v-model="form.email"
        type="email"
        :class="['form-control pb_height-50 reverse', errors.email ? 'border border-danger rounded' : '']"
        placeholder="Email"
        autocomplete="email"
      />
      <span v-if="errors.email" class="d-inline-block ml-1" :title="errors.email">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group d-flex align-items-center">
      <input
        v-model="form.senha"
        type="password"
        :class="['form-control pb_height-50 reverse', errors.senha ? 'border border-danger rounded' : '']"
        placeholder="Senha"
        autocomplete="new-password"
      />
      <span v-if="errors.senha" class="d-inline-block ml-1" :title="errors.senha">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group d-flex align-items-center">
      <input
        v-model="form.valor"
        type="text"
        inputmode="decimal"
        :class="['form-control pb_height-50 reverse', errors.valor ? 'border border-danger rounded' : '']"
        placeholder="Valor da compra (R$ 0,00)"
        autocomplete="off"
        @input="formatValor"
      />
      <span v-if="errors.valor" class="d-inline-block ml-1" :title="errors.valor">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group d-flex align-items-center">
      <div :class="['d-flex flex-column fileUpload', errors.comprovante ? 'border border-danger rounded' : '']">
        <label for="txtAnexarComprovante" class="mb-0 ml-3" style="font-size: 0.85rem; cursor: pointer">
          <i class="fa fa-folder-open mr-1"></i>
          {{ fileName || 'Anexar comprovante da compra' }}
        </label>
        <input
          id="txtAnexarComprovante"
          ref="fileInputRef"
          type="file"
          accept="image/*"
          class="upload"
          @change="handleFileUpload"
        />
      </div>
      <span v-if="errors.comprovante" class="d-inline-block ml-1" :title="errors.comprovante">
        <i class="fa fa-info-circle text-danger" aria-hidden="true"></i>
      </span>
    </div>

    <div class="form-group">
      <div class="form-check">
        <input
          v-model="form.aceito"
          type="checkbox"
          :class="['form-check-input', errors.aceito ? 'border border-danger' : '']"
          id="termos"
        />
        <label class="form-check-label" for="termos">
          Eu aceito os
          <a href="https://quantashop.com.br/termos" target="_blank" rel="noreferrer" style="color: #665fee; font-weight: 500">
            termos e condições
          </a>
        </label>
      </div>
      <small v-if="errors.aceito" class="text-danger d-block mt-1">{{ errors.aceito }}</small>
    </div>

    <div class="form-group">
      <button type="submit" class="btn btn-primary btn-lg btn-block" :disabled="loading">
        <span v-if="loading">
          <span class="pc-spinner"></span>
          Enviando...
        </span>
        <span v-else>Cadastrar</span>
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { toast } from 'vue3-toastify'

const props = defineProps<{ cnpj?: string }>()

const { post } = useApi()

const fileInputRef = ref<HTMLInputElement | null>(null)
const fileName = ref('')
const loading = ref(false)

const form = reactive({
  cpf: '',
  whatsapp: '',
  email: '',
  senha: '',
  valor: '',
  aceito: false,
  comprovante: '',
})

const errors = reactive<Record<string, string>>({})

function keepOnlyNumbers(str: string) {
  return str ? str.replace(/\D/g, '') : str
}

function parseCurrency(str: string): number {
  if (!str) return 0
  const clean = str.replace(/\./g, '').replace(',', '.')
  return parseFloat(clean) || 0
}

function formatValor(e: Event) {
  const input = e.target as HTMLInputElement
  let raw = input.value.replace(/\D/g, '')
  if (!raw) { form.valor = ''; return }
  const num = parseInt(raw, 10) / 100
  form.valor = num.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}

function handleFileUpload(e: Event) {
  const input = e.target as HTMLInputElement
  const file = input.files?.[0]
  if (!file) return
  fileName.value = file.name
  const reader = new FileReader()
  reader.onload = () => { form.comprovante = reader.result as string }
  reader.readAsDataURL(file)
}

function validateForm(): boolean {
  Object.keys(errors).forEach(k => delete errors[k])
  const cpfClean = keepOnlyNumbers(form.cpf)
  const whatsClean = keepOnlyNumbers(form.whatsapp)
  const valorNum = parseCurrency(form.valor)

  if (!/^\d{11}$/.test(cpfClean)) errors.cpf = 'CPF é obrigatório e deve ter 11 dígitos.'
  if (!/^\d{10,11}$/.test(whatsClean)) errors.whatsapp = 'WhatsApp é obrigatório e deve ter entre 10 e 11 dígitos.'
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) errors.email = 'Email é obrigatório e deve ser válido.'
  if (valorNum <= 0) errors.valor = 'Valor da compra é obrigatório e deve ser maior que 0.'
  if (!/^(?=.*[a-zA-Z])(?=.*\d).{8,}$/.test(form.senha)) errors.senha = 'A senha deve ter pelo menos 8 caracteres, incluindo letras e números.'
  if (!form.aceito) errors.aceito = 'Você deve aceitar os termos e condições.'
  if (!form.comprovante) errors.comprovante = 'Você deve enviar o comprovante da compra.'

  return Object.keys(errors).length === 0
}

async function handleSubmit() {
  if (!validateForm()) return
  loading.value = true

  const payload = {
    cpf: keepOnlyNumbers(form.cpf),
    celular: keepOnlyNumbers(form.whatsapp),
    email: form.email,
    valorCompra: parseCurrency(form.valor),
    senha: form.senha,
    comprovanteCompra: form.comprovante,
    cnpj: props.cnpj || null,
  }

  try {
    await post('/user/primeiraCompra', payload)
    Object.assign(form, { cpf: '', whatsapp: '', email: '', senha: '', valor: '', aceito: false, comprovante: '' })
    fileName.value = ''
    if (fileInputRef.value) fileInputRef.value.value = ''
    toast.success('Cadastro realizado com sucesso!')
  } catch (err: any) {
    const data = err?.response?.data
    const msg = data?.erros?.[0]?.mensagem || 'Erro ao realizar o cadastro'
    toast.error(msg)
  } finally {
    loading.value = false
  }
}

</script>

<style scoped>
.pc-spinner {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: pc-spin .7s linear infinite;
  vertical-align: middle;
  margin-right: 6px;
}
@keyframes pc-spin { to { transform: rotate(360deg); } }
</style>
