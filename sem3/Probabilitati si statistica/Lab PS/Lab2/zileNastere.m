function contor = zileNastere(nrPers, zile, repetitii)
  contor = 0;
  for i = 1 : repetitii
    if length(unique(randi(zile,1,nrPers))) < nrPers
      contor++;
    endif
  endfor
  contor = contor/repetitii;
endfunction
% calculul teoretic
% cazuri pos: 365^23
% cazuri nefav: toti au zile diferite => A de 365 luate cate 23
% P = (cazuri pos - cazuri nefav)/cazuri pos