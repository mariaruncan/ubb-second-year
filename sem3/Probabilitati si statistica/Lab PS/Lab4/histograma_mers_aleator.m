clf; grid on; hold on;
p = 0.7; nrpasi = 10; nr_sim = 1000;
% x = pozitii finale din simulari
x = zeros(1,nr_sim);
pasi_dr = zeros(1,nr_sim);
for i=1:nr_sim
    [pozitii_vizitate, nr_p] = mers_aleator(nrpasi, p);
    x(i) = pozitii_vizitate(end);
    pasi_dr(i) = nr_p;
endfor
N = histc(x, -nrpasi:nrpasi);
bar(-nrpasi:nrpasi, N/nr_sim,'hist','FaceColor','b');
xticks(-nrpasi:nrpasi);
axis([-nrpasi-1 nrpasi+1 0 0.5]);

fprintf('Poz. finale cele mai frecvente: %d.\n',find(N==max(N))-nrpasi-1);
fprintf('Valoarea medie me pasi la dreapta: %d.\n', mean(pasi_dr));
