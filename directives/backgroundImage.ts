import type { DirectiveBinding } from 'vue';

export default {
  mounted(el: HTMLElement, binding: DirectiveBinding) {
    const url = binding.value as string;
    const styleId = 'dynamic-background-style';

    // Adiciona uma classe ao elemento
    el.classList.add(`dynamic-background-${Math.random().toString(36).substr(2, 9)}`);

    // Cria uma regra de estilo para o pseudo-elemento ::before
    const styleElement = document.createElement('style');
    styleElement.id = styleId;
    styleElement.textContent = `
      .dynamic-background::before {
        content: " ";
        display: block;
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        opacity: 0.2;
        background-image: url('${url}');
        background-repeat: no-repeat;
      }
    `;
    
    document.head.appendChild(styleElement);
  },

};
