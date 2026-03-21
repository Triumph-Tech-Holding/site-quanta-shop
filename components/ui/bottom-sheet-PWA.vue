<template>
  <div
    v-if="showBottomSheet && !isIosDevice && !isAndroidDevice"
    class="bottom-sheet"
  >
    <div class="bottom-sheet-content">
      <img src="/img/logo/logo.png" alt="Quanta Shop" />
      <h2 class="text-center text-uppercase">Instale o Quanta Shop</h2>
      <p class="text-center">
        O Quanta Shop é a plataforma ideal para lojistas que desejam aumentar
        suas vendas e recompensar seus clientes com cashback. Para aproveitar ao
        máximo todos os benefícios, você pode instalar o aplicativo diretamente
        no seu dispositivo e acessá-lo de forma rápida e prática, sempre que
        precisar!
      </p>
      <button @click="installPWA" class="btn-primary">Instalar</button>
      <button @click="closeBottomSheet" class="btn-secondary">Fechar</button>
    </div>
  </div>
  <div
    v-if="showBottomSheet && isIosDevice"
    id="bottomSheetIos"
    class="bottom-sheet"
  >
    <div class="bottom-sheet-content">
      <img src="/img/logo/logo.png" alt="Quanta Shop" />
      <h2 class="text-center text-uppercase">Instale o Quanta Shop</h2>
      <p class="text-center">
        Para instalar o Quanta Shop no iOS, clique no botão de compartilhar
        <svg-share /> e selecione "Adicionar à Tela de Início".
      </p>
      <button @click="installPWA" class="btn-primary">Instalar</button>
      <button @click="closeBottomSheet" class="btn-secondary">Fechar</button>
    </div>
  </div>

  <div v-if="showBottomSheet && isAndroidDevice" class="bottom-sheet">
    <div class="bottom-sheet-content">
      <img src="/img/logo/logo.png" alt="Quanta Shop" />
      <h2 class="text-center text-uppercase">Instale o Quanta Shop</h2>
      <p class="text-center">
        Para instalar o Quanta Shop no Android, clique no botão de opções no
        Chrome, em seguida "Adicionar à tela inicial" e selecione "Instalar".
      </p>
      <button @click="installPWA" class="btn-primary">Instalar</button>
      <button @click="closeBottomSheet" class="btn-secondary">Fechar</button>
    </div>
  </div>
</template>

<script setup>
const installPromptEvent = ref(null);
const showBottomSheet = ref(false);
const isIosDevice = ref(false);
const isAndroidDevice = ref(false);

const isIos = () => {
  const userAgent = window.navigator.userAgent;
  return /iPhone|iPad|iPod/i.test(userAgent);
};

const isAndroid = () => {
  const userAgent = window.navigator.userAgent.toLowerCase();
  return /android/.test(userAgent);
};

onMounted(() => {
  isIosDevice.value = isIos();
  isAndroidDevice.value = isAndroid();

  // Se já está instalado como PWA, não mostra
  if (window.matchMedia("(display-mode: standalone)").matches) {
    showBottomSheet.value = false;
    return;
  }

  // Verifica se foi fechado nesta sessão (aba atual)
  if (sessionStorage.getItem("pwaBottomSheetDismissed")) {
    showBottomSheet.value = false;
    return;
  }

  // Verifica se foi fechado nos últimos 90 dias
  const lastClosedDate = localStorage.getItem("pwaBottomSheetClosed");
  if (lastClosedDate) {
    const daysSinceClosed = (Date.now() - new Date(lastClosedDate).getTime()) / (1000 * 60 * 60 * 24);
    if (daysSinceClosed < 90) {
      showBottomSheet.value = false;
      return;
    }
  }

  // Mostra o popup (mas só para dispositivos móveis ou se recebeu o evento beforeinstallprompt)
  if (isIosDevice.value || isAndroidDevice.value) {
    showBottomSheet.value = true;
  }

  window.addEventListener("beforeinstallprompt", (e) => {
    e.preventDefault();
    installPromptEvent.value = e;
    showBottomSheet.value = true;
  });
});

const installPWA = async () => {
  if (isIosDevice.value) {
    alert("Para instalar no iOS, adicione à tela inicial pelo botão de compartilhar.");
  } else if (isAndroidDevice.value) {
    alert("Para instalar no Android, clique no botão de opções no Chrome, em seguida 'Adicionar à tela inicial' e selecione 'Instalar'.");
  } else if (installPromptEvent.value) {
    installPromptEvent.value.prompt();
    const choiceResult = await installPromptEvent.value.userChoice;
    if (choiceResult.outcome === "accepted") {
      console.log("Usuário aceitou a instalação do PWA");
    } else {
      console.log("Usuário recusou a instalação do PWA");
    }
    installPromptEvent.value = null;
    showBottomSheet.value = false;
  }
};

const closeBottomSheet = () => {
  const currentDate = new Date().toISOString();
  localStorage.setItem("pwaBottomSheetClosed", currentDate);
  sessionStorage.setItem("pwaBottomSheetDismissed", "1");
  showBottomSheet.value = false;
};
</script>

<style scoped>
.bottom-sheet {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: white;
  box-shadow: 0px -2px 10px rgba(0, 0, 0, 0.3);
  border-top-left-radius: 16px;
  border-top-right-radius: 16px;
  padding: 20px;
  z-index: 100;
  animation: slideUp 0.3s ease-out;
}

.bottom-sheet-content {
  text-align: center;
}

.bottom-sheet-content img,
#bottomSheetIos img {
  width: 120px;
  margin-bottom: 30px;
}

.bottom-sheet-content h2 {
  font-size: 1rem;
  color: #1e5d68;
}

.bottom-sheet-content p {
  text-align: justify;
  line-height: normal;
}

.bottom-sheet button.btn-primary {
  margin: 10px;
  padding: 10px 20px;
  background-color: #1e5d68;
  text-transform: uppercase;
  border: none;
  color: white;
  border-radius: 4px;
  cursor: pointer;
}

.bottom-sheet button.btn-secondary {
  margin: 10px;
  padding: 10px 20px;
  background-color: #ffffff;
  text-transform: uppercase;
  border: 1px solid #1e5d68;
  color: #1e5d68;
  border-radius: 4px;
  cursor: pointer;
}

@keyframes slideUp {
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(0);
  }
}
</style>
