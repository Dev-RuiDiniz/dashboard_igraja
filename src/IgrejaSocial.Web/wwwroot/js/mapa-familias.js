window.mapaFamilias = {
  mapa: null,
  marcadores: [],
  iniciar: function (elementId, familias) {
    if (!window.L) {
      return;
    }

    if (this.mapa) {
      this.mapa.remove();
    }

    this.mapa = L.map(elementId).setView([-23.5505, -46.6333], 11);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
      attribution: "&copy; OpenStreetMap contributors"
    }).addTo(this.mapa);

    this.atualizarPins(familias);
  },
  atualizarPins: function (familias) {
    if (!this.mapa) {
      return;
    }

    this.marcadores.forEach(marker => marker.remove());
    this.marcadores = [];

    const bounds = [];

    familias.forEach(familia => {
      if (familia.latitude == null || familia.longitude == null) {
        return;
      }

      const marker = L.marker([familia.latitude, familia.longitude])
        .addTo(this.mapa)
        .bindPopup(`<strong>${familia.nomeResponsavel}</strong><br/>${familia.endereco}`);
      this.marcadores.push(marker);
      bounds.push([familia.latitude, familia.longitude]);
    });

    if (bounds.length > 0) {
      this.mapa.fitBounds(bounds, { padding: [20, 20] });
    }
  }
};
