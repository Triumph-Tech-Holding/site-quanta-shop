import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useUserStore } from '@/pinia/useUserStore';
import { useLoadingStore } from '@/pinia/useLoadingStore';
import {
    getPartners,
    getNewPartners, getFeaturedPartners, getTopSellersPartners,
    getLocalPartners, getBestDiscountsLocalPartners, getFeaturedLocalPartners, getTopSellersLocalPartners
}
    from "@/services/partner-service";

function normalizeOnlinePartner(p: Record<string, unknown>) {
    return {
        id: p['IdAnunciante'],
        nome: p['Nome'],
        imagem: p['Icone'],
        imagemPequena: p['Icone'],
        percentualCashback: p['MaxCashback'] ?? p['MinCashback'],
        link: p['link'],
        categoria: p['Categoria'],
        tipo: p['Tipo'],
    };
}

function normalizeLocalPartner(p: Record<string, unknown>) {
    return {
        id: p['IdCredenciamento'] ?? p['IdAnunciante'] ?? p['id'],
        nome: p['Estabelecimento'] ?? p['Nome'] ?? p['nome'],
        imagem: p['Imagem'] ?? p['Icone'] ?? p['imagem'],
        imagemPequena: p['Imagem'] ?? p['Icone'] ?? p['imagemPequena'],
        percentualCashback: p['PercentualCashback'] ?? p['percentualCashback'],
        bairro: p['Bairro'] ?? p['bairro'],
        cidade: p['CidadeNome'] ?? p['Cidade'] ?? p['cidade'],
        estado: p['EstadoNome'] ?? p['Estado'] ?? p['estado'],
        whatsapp: p['CelularContato'] ?? p['WhatsApp'] ?? p['Whatsapp'] ?? p['whatsapp'],
        link: p['link'],
    };
}

export const usePartnerStore = defineStore('partners', () => {
    const userStore = useUserStore();
    const loadingStore = useLoadingStore();
    const partners = ref<Array<any>>([]);
    const newPartners = ref<Array<any>>([]);
    const featuredPartners = ref<Array<any>>([]);
    const topSellersPartners = ref<Array<any>>([]);
    const localPartners = ref<Array<any>>([]);
    const bestDiscountsLocalPartners = ref<Array<any>>([]);
    const featuredLocalPartners = ref<Array<any>>([]);
    const topSellersLocalPartners = ref<Array<any>>([]);

    const isPartnersLoaded = ref(false);
    const isNewPartnersLoaded = ref(false);
    const isFeaturedPartnersLoaded = ref(false);
    const isTopSellersPartnersLoaded = ref(false);
    const isLocalPartnersLoaded = ref(false);
    const isBestDiscountsLocalPartnersLoaded = ref(false);
    const isFeaturedLocalPartnersLoaded = ref(false);
    const isTopSellersLocalPartnersLoaded = ref(false);

    async function fetchPartners(type = null, name = null, category = null, page = 1, limit = 12) {
        loadingStore.setLoading(true);

        try {
            const response = await getPartners(type, name, category, page, limit);

            partners.value = response.data;

            await userStore.loadUserFromStorage();

            partners.value.forEach(partner => {
                partner.link = userStore.isLoggedIn ? partner.link.replace('{userId}', userStore.userId) : null;
            });

            isPartnersLoaded.value = true;
        } catch (error) {
            console.error('Erro ao buscar parceiros:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchNewPartners() {
        if (isNewPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (newPartners.value.length == 0) {
                const response = await getNewPartners();

                newPartners.value = (response.data as Record<string, unknown>[]).map(normalizeOnlinePartner);

                isNewPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar novos parceiros:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchFeaturedPartners() {
        if (isFeaturedPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (featuredPartners.value.length == 0) {
                const response = await getFeaturedPartners();

                featuredPartners.value = (response.data as Record<string, unknown>[]).map(normalizeOnlinePartner);

                isFeaturedPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar parceiros em destaque:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchTopSellersPartners() {
        if (isTopSellersPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (topSellersPartners.value.length == 0) {
                const response = await getTopSellersPartners();

                topSellersPartners.value = response.data;

                isTopSellersPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar parceiros que mais vendem:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchLocalPartners() {
        if (isLocalPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (localPartners.value.length == 0) {
                const response = await getLocalPartners();

                localPartners.value = (response.data as Record<string, unknown>[]).map(normalizeLocalPartner);

                await userStore.loadUserFromStorage();

                localPartners.value.forEach(partner => {
                    if (partner.link && partner.link.includes('{userId}')) {
                        partner.link = userStore.isLoggedIn ? partner.link.replace('{userId}', userStore.userId) : null;
                    }
                });

                isLocalPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar parceiros locais:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchBestDiscountsLocalPartners() {
        if (isBestDiscountsLocalPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (bestDiscountsLocalPartners.value.length == 0) {
                const response = await getBestDiscountsLocalPartners();

                bestDiscountsLocalPartners.value = response.data;

                await userStore.loadUserFromStorage();

                bestDiscountsLocalPartners.value.forEach(partner => {
                    if (partner.link && partner.link.includes('{userId}')) {
                        partner.link = userStore.isLoggedIn ? partner.link.replace('{userId}', userStore.userId) : null;
                    }
                });

                isBestDiscountsLocalPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar melhores descontos de parceiros locais:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchFeaturedLocalPartners() {
        if (isFeaturedLocalPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (featuredLocalPartners.value.length == 0) {
                const response = await getFeaturedLocalPartners();

                featuredLocalPartners.value = response.data;

                await userStore.loadUserFromStorage();

                featuredLocalPartners.value.forEach(partner => {
                    if (partner.link && partner.link.includes('{userId}')) {
                        partner.link = userStore.isLoggedIn ? partner.link.replace('{userId}', userStore.userId) : null;
                    }
                });

                isFeaturedLocalPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar parceiros locais em destaque:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    async function fetchTopSellersLocalPartners() {
        if (isTopSellersLocalPartnersLoaded.value) return;
        
        loadingStore.setLoading(true);

        try {
            if (topSellersLocalPartners.value.length == 0) {
                const response = await getTopSellersLocalPartners();

                topSellersLocalPartners.value = response.data;

                await userStore.loadUserFromStorage();

                topSellersLocalPartners.value.forEach(partner => {
                    if (partner.link && partner.link.includes('{userId}')) {
                        partner.link = userStore.isLoggedIn ? partner.link.replace('{userId}', userStore.userId) : null;
                    }
                });

                isTopSellersLocalPartnersLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar parceiros locais mais vendidos:', error);
        }
        finally {
            loadingStore.setLoading(false);
        }
    }

    return {
        // Partners
        partners, newPartners, featuredPartners, topSellersPartners,
        isPartnersLoaded, isNewPartnersLoaded, isFeaturedPartnersLoaded, isTopSellersPartnersLoaded,
        fetchPartners, fetchNewPartners, fetchFeaturedPartners, fetchTopSellersPartners,

        // Local partners
        localPartners, bestDiscountsLocalPartners, featuredLocalPartners, topSellersLocalPartners,
        isLocalPartnersLoaded, isBestDiscountsLocalPartnersLoaded, isFeaturedLocalPartnersLoaded, isTopSellersLocalPartnersLoaded,
        fetchLocalPartners, fetchBestDiscountsLocalPartners, fetchFeaturedLocalPartners, fetchTopSellersLocalPartners
    }
});