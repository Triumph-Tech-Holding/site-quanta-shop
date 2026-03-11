export default {
    mounted(el: any, binding: any) {
      // Lógica da máscara
      el.value = applyMask(el.value, binding.value); // Exemplo de aplicação de máscara
      el.addEventListener('input', (e : any) => {
        e.target.value = applyMask(e.target.value, binding.value);
      });
    }
}
  
  function applyMask(value: any, mask: any) {
    // Função que aplica a máscara (pode ser customizada)
    return value; // Retorne o valor com a máscara aplicada
  }
  