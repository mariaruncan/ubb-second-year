% x = normrnd(m, s, l, c)
% m - media
% s - sigma = deviatia standard
% l - nr linii
% c - nr coloane
nr_pers = 10000;
x = normrnd(165, 10, 1, nr_pers);
% hist(x);
nr_bare = 20;
[~, mijloace_bare] = hist(x, nr_bare);

% norm = 1/L = nr_bare/[max(x)-min(x)]

norm = nr_bare / (max(x) - min(x));

clf
hold on;
hist(x, mijloace_bare, norm)
diviziune = linspace(min(x), max(x), 1000);
valori = normpdf(diviziune, 165, 10);
plot(diviziune, valori, '-r', 'linewidth', 2)
axis([120, 210, 0, 0.045])

mean(x)
std(x)
std(x, 1)


mean(x > 160 & x < 170) % practic
normcdf(170, 165, 10) - normcdf(160, 165, 10) % teoretic
