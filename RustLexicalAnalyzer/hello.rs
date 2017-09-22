pub fn fn_validate(text: &str) {
	let mut out = io::stdout();
	let x: f64 = text.parse().unwrap();
	let f64_bytes: u64 = unsafe { transmute(x) };
	let x: f32 = text.parse().unwrap();
	let f32_bytes: u32 = unsafe { transmute(x) };
	writeln!(&mut out, "{:016x} {:08x} {}", f64_bytes, f32_bytes, text).unwrap();
}