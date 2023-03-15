function [pozitii_vizitate,pasi_dreapta] = mers_aleator(nrpasi, p)
  pozitii_vizitate = zeros(1, nrpasi+1);
  pasi_dreapta = 0;
  for i = 1:nrpasi
    if rand <= p
      pas = 1;
      pasi_dreapta++;
    else
      pas = -1;
    endif
    pozitii_vizitate(i+1) = pozitii_vizitate(i) + pas;
  endfor
endfunction