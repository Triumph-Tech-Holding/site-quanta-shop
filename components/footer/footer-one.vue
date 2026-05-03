<template>
  <footer>
    <div :class="`tp-footer-area ${primary_style?'tp-footer-style-2 tp-footer-style-primary tp-footer-style-6':''} ${style_2 ?'tp-footer-style-2':style_3 ? 'tp-footer-style-2 tp-footer-style-3': ''}`" :data-bg-color="`${style_2?'footer-bg-white':style_3?'footer-bg-white':'footer-bg-grey'}`">
      <div class="tp-footer-top pt-95 pb-40">
        <div class="container">
          <div class="row">
            <div class="col-xl-4 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-1 mb-50">
                <div class="tp-footer-widget-content">
                  <div class="tp-footer-logo">
                    <nuxt-link href="/">
                      <img src="/img/logo/logo.png" alt="logo">
                    </nuxt-link>
                  </div>
                  <p class="tp-footer-desc">Somos uma equipe dedicada a oferecer as melhores oportunidades de economia e renda através de um consumo inteligente.
                  </p>
                  <div class="tp-footer-social">
                    <!-- social links -->
                    <footer-social-links/>
                    <!-- social links -->
                  </div>
                </div>
              </div>
            </div>
            <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-2 mb-50">
                <h4 class="tp-footer-widget-title">Minha conta</h4>
                <div class="tp-footer-widget-content">
                  <ul>
                    <li><nuxt-link to="/agencia/painel/minhas-compras">Minhas compras</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/financeiro">Meu extrato</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/minha-rede">Meus indicados</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/meus-dados">Minha conta</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/assinatura">Assinatura</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/assinatura">Assistente virtual</nuxt-link></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-3 mb-50">
                <h4 class="tp-footer-widget-title">Informação</h4>
                <div class="tp-footer-widget-content">
                  <ul>
                    <li><a href="about">Quem somos</a></li>
                    <li><nuxt-link to="/agencia/como-funciona">Como funciona</nuxt-link></li>
                    <li><nuxt-link to="/agencia/cadastro">Credenciamento</nuxt-link></li>
                    <li><a href="https://bigcash.blob.core.windows.net/documentos/QB%20-%20TERMOS%20DE%20USO%20E%20BONIFICA%C3%87%C3%83O%20ESPEC%C3%8DFICA%20v102023.pdf" target="_blank">Termos & condições</a></li>
                    <li><a href="blog">Últimas notícias</a></li>
                    <li><a href="contact">Contato</a></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-4 mb-50">
                <h4 class="tp-footer-widget-title">Atendimento</h4>
                <div class="tp-footer-widget-content">
                  <ul>
                    <li><a href="https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0" target="_blank">WhatsApp</a></li>
                    <li><nuxt-link to="/contato">Central de ajuda</nuxt-link></li>
                    <li><nuxt-link to="/agencia/faq">Perguntas frequentes</nuxt-link></li>
                    <li><nuxt-link to="/agencia/privacidade">Política de privacidade</nuxt-link></li>
                    <li><a href="https://bigcash.blob.core.windows.net/documentos/QB%20-%20TERMOS%20DE%20USO%20E%20BONIFICA%C3%87%C3%83O%20ESPEC%C3%8DFICA%20v102023.pdf" target="_blank">Termos de uso</a></li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- bottom area start -->
      <footer-bottom-area/>
      <!-- bottom area end -->
  </div>
</footer>
</template>

<script setup lang="ts">
defineProps<{ primary_style?: boolean; style_2?: boolean;style_3?: boolean; }>();
</script>
