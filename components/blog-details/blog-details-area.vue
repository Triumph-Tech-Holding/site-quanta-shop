<template>
  <section class="tp-postbox-details-area pb-120 pt-95">
    <div class="container">
      <div class="row">
        <div class="col-xl-9">
          <div class="tp-postbox-details-top">
            <div class="tp-postbox-details-category">
              <span>
                <a href="#">{{ blog.category }}</a>
              </span>
            </div>
            <h3 class="tp-postbox-details-title">{{ blog.title }}</h3>
            <div class="tp-postbox-details-meta mb-50">
              <span data-meta="author">
                <svg-user-2 />
                Por <a href="#">{{ blog.author }}</a>
              </span>
              <span>
                <svg-date />
                {{ blog.date }}
              </span>
              <span>
                <svg-comments />
                <a href="#">Comentários ({{ blog.comments }})</a>
              </span>
            </div>
          </div>
        </div>
        <div class="col-xl-12">
          <div class="tp-postbox-details-thumb">
            <img :src="`/img/blog/details/blog-big-${blog.id}.jpg`" alt="" />
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-xl-9 col-lg-8">
          <div class="tp-postbox-details-main-wrapper">
            <div class="tp-postbox-details-content">
              <div v-html="blog.htmlContent"></div>

              <!-- details navigation start -->
              <!-- <blog-details-navigation /> -->
              <!-- details navigation end -->

              <div
                class="tp-postbox-details-author d-sm-flex align-items-start"
                data-bg-color="#F4F7F9"
                style="background-color: #f4f7f9"
              >
                <div class="tp-postbox-details-author-thumb">
                  <a href="#">
                    <img src="/img/users/user-12.jpg" alt="" />
                  </a>
                </div>
                <div class="tp-postbox-details-author-content">
                  <span>Escrito por</span>
                  <h5 class="tp-postbox-details-author-title">
                    <a href="#">{{ blog.author }}</a>
                  </h5>
                  <!-- <p>
                    By defining and following internal and external processes,
                    your team will have clarity on resources to attract and
                    retain customers for your business.
                  </p> -->

                  <div class="tp-postbox-details-author-social">
                    <a href="#"><i class="fa-brands fa-facebook-f"></i></a>
                    <a href="#"><i class="fa-brands fa-instagram"></i></a>
                    <a href="#"><i class="fa-brands fa-whatsapp"></i></a>
                    <a href="#"><i class="fa-brands fa-youtube"></i></a>
                  </div>
                </div>
              </div>

              <!-- blog comment start -->
              <!-- <blog-details-comments /> -->
              <!-- blog comment end -->

              <!--  form start -->
              <!-- <forms-blog-reply-form /> -->
              <!--  form end -->
            </div>
          </div>
        </div>
        <div class="col-xl-3 col-lg-4">
          <!-- blog sidebar start -->
          <blog-sidebar />
          <!-- blog sidebar end -->
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { type IBlogType } from "@/types/blog-type";

defineProps<{ blog: IBlogType }>();
</script>
