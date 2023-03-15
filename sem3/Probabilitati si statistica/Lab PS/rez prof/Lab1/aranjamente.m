function aranjamente = aranjamente(v,k)
	combinari = nchoosek(v, k); % combinari de length(v) luate cate k din vectorul v
	aranjamente=[ ];
	for i = 1:nchoosek(length(v),k)
    aranjamente=[aranjamente;perms(combinari(i,:))]; % ; este ca un fel de + la concatenare
end
% nchoosek(nr, k) - nr efectiv de combinari
% nchoosek(vector, k) - combinarile