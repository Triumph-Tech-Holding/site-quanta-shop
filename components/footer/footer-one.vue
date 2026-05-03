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
                      <img src="/img/logo/logo.png" alt="logo" loading="lazy">
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
                <h4 class="tp-footer-widget-title">Links Úteis</h4>
                <div class="tp-footer-widget-content">
                  <ul>
                    <li><nuxt-link to="/agencia/painel/minhas-compras">Minhas compras</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/financeiro">Meu extrato</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/minha-rede">Meus indicados</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/meus-dados">Minha conta</nuxt-link></li>
                    <li><nuxt-link to="/agencia/painel/assinatura">Planos</nuxt-link></li>
                    <li><nuxt-link to="/agencia/faq">Perguntas frequentes</nuxt-link></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-3 mb-50">
                <h4 class="tp-footer-widget-title">Parceiros</h4>
                <div class="tp-footer-widget-content">
                  <ul>
                    <li><nuxt-link to="/partners">Lojas online</nuxt-link></li>
                    <li><nuxt-link to="/agencia/lojas-fisicas">Lojas físicas</nuxt-link></li>
                    <li><nuxt-link to="/agencia/cadastro">Seja um parceiro</nuxt-link></li>
                    <li><nuxt-link to="/agencia/como-funciona">Como funciona</nuxt-link></li>
                    <li><a href="/about">Quem somos</a></li>
                    <li><a href="/blog">Notícias</a></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-6">
              <div class="tp-footer-widget footer-col-4 mb-50">
                <h4 class="tp-footer-widget-title">Redes Sociais</h4>
                <div class="tp-footer-widget-content">
                  <ul class="qs-footer-social-list">
                    <li>
                      <a href="https://www.instagram.com/quantashopoficial" target="_blank" rel="noopener">
                        <i class="fa-brands fa-instagram"></i> Instagram
                      </a>
                    </li>
                    <li>
                      <a href="https://www.facebook.com/quantabank" target="_blank" rel="noopener">
                        <i class="fa-brands fa-facebook-f"></i> Facebook
                      </a>
                    </li>
                    <li>
                      <a href="https://wa.me/message/QFVP2ILERA4LD1" target="_blank" rel="noopener">
                        <i class="fa-brands fa-whatsapp"></i> WhatsApp
                      </a>
                    </li>
                    <li>
                      <a href="https://www.youtube.com/@quantabank" target="_blank" rel="noopener">
                        <i class="fa-brands fa-youtube"></i> YouTube
                      </a>
                    </li>
                  </ul>
                  <div class="qs-footer-seal mt-20">
                    <span class="qs-footer-seal__badge">
                      <i class="fa-solid fa-lock"></i> SSL Seguro
                    </span>
                    <span class="qs-footer-seal__badge">
                      <i class="fa-solid fa-shield-halved"></i> Dados protegidos
                    </span>
                  </div>
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
