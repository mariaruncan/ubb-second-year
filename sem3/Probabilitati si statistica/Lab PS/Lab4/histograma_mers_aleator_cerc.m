clf; grid on; hold on;
p = 0.2; noduri = 5; nrpasi = 10; nr_sim = 1000;
% x = pozitii finale din simulari
x = zeros(1,nr_sim);
pasi_dr = zeros(1,nr_sim);
for i=1:nr_sim
    [pozitii_vizitate, nr_p] = mers_aleator(nrpasi, p);
    x(i) = mod(pozitii_vizitate(end),noduri);
    pasi_dr(i) = nr_p;
endfor
N = histc(x, 0:noduri-1);
bar(0:noduri-1, N/nr_sim,'hist','FaceColor','b');
xticks(0:noduri-1);
axis([-1 noduri+1 0 0.3]);

fprintf('Poz. finale cele mai frecvente: %d.\n',find(N==max(N))-1);
fprintf('Valoarea medie me pasi la dreapta: %d.\n', mean(pasi_dr));
